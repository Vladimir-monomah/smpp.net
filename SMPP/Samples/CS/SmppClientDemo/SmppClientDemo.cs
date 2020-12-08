using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inetlab.SMPP;
using Inetlab.SMPP.Builders;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;
using SmppLogger;

namespace SmppClientDemo
{

	public partial class SmppClientDemo :Form
	{
        private readonly MessageComposer _messageComposer;
        private readonly ILog _log;

		private readonly SmppClient _client;


	    public SmppClientDemo()
		{
            this.InitializeComponent();

            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogClient.txt");
            LogManager.SetLoggerFactory(new TextBoxLogFactory(this.tbLog, LogLevel.Info, fileName));


            this._log = LogManager.GetLogger(this.GetType().Name);


            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogManager.GetLogger("Домен приложения").Fatal((Exception)args.ExceptionObject, "Необработанное исключение");
            };


            this._client = new SmppClient();
            this._client.ResponseTimeout = TimeSpan.FromSeconds(60);
            this._client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

            this._client.evDisconnected += new DisconnectedEventHandler(this.client_evDisconnected);
            this._client.evDeliverSm += new DeliverSmEventHandler(this.client_evDeliverSm);
            this._client.evEnquireLink += new EnquireLinkEventHandler(this.client_evEnquireLink);
            this._client.evUnBind += new UnBindEventHandler(this.client_evUnBind);
            this._client.evDataSm += new DataSmEventHandler(this.client_evDataSm);
            this._client.evRecoverySucceeded += this.ClientOnRecoverySucceeded;

            this._client.evServerCertificateValidation += this.OnCertificateValidation;


            this._messageComposer = new MessageComposer();
            this._messageComposer.evFullMessageReceived += this.OnFullMessageReceived;
            this._messageComposer.evFullMessageTimeout += this.OnFullMessageTimeout;
            
		}


        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
	    {
            //принять все сертификаты
            args.Accepted = true;
	    }


        /// <summary>
        /// Очистите все используемые ресурсы.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (this.components != null) 
				{
                    this.components.Dispose();
				}

                this._client.Dispose();
			}
			base.Dispose( disposing );
		}

	

		private async Task Connect()
		{
            if (this._client.Status == ConnectionStatus.Closed)
			{
                this._log.Info("Присоединенный к " + this.tbHostname.Text);

                this.bConnect.Enabled = false;
                this.bDisconnect.Enabled = false;
                this.cbReconnect.Enabled = false;


                this._client.EsmeAddress = new SmeAddress("", (AddressTON)Convert.ToByte(this.tbAddrTon.Text), (AddressNPI)Convert.ToByte(this.tbAddrNpi.Text));
                this._client.SystemType   = this.tbSystemType.Text;

                this._client.ConnectionRecovery = this.cbReconnect.Checked;
                this._client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);
			

                if (this.cbSSL.Checked)
			    {
                    this._client.EnabledSslProtocols = SslProtocols.Default;
                    this._client.ClientCertificates.Clear();
                    this._client.ClientCertificates.Add(new X509Certificate2("client.p12", "12345"));
			    }
			    else
			    {
                    this._client.EnabledSslProtocols = SslProtocols.None;
			    }

				bool bSuccess = await this._client.ConnectAsync(this.tbHostname.Text, Convert.ToInt32(this.tbPort.Text));

			    if (bSuccess)
			    {
                    this._log.Info("SmppClient подключен");

			       await this.Bind();
			    }
			    else
			    {

                    this.bConnect.Enabled = true;
                    this.cbReconnect.Enabled = true;
                    this.bDisconnect.Enabled = false;
			    }
            }

		}

	    private async Task Bind()
	    {
            this._log.Info("Связать клиента с SystemId: {0}", this.tbSystemId.Text);

	        ConnectionMode mode = ConnectionMode.Transceiver;

            this.bDisconnect.Enabled = true;
	        mode = (ConnectionMode)this.cbBindingMode.SelectedItem;


	        BindResp resp = await this._client.BindAsync(this.tbSystemId.Text, this.tbPassword.Text, mode);

	        switch (resp.Header.Status)
	        {
	            case CommandStatus.ESME_ROK:
                    this._log.Info("Связывание выполнено успешно: статус: {0}, SystemId: {1}", resp.Header.Status, resp.SystemId);

                    this.bSubmit.Enabled = true;

	                break;
	            default:
                    this._log.Warn("Ошибка привязки: статус: {0}", resp.Header.Status);

                    await this.Disconnect();
	               break;
	        }
	    }





	    private async Task Disconnect()
		{
            this._log.Info("Отключиться от сервера SMPP");

            if (this._client.Status == ConnectionStatus.Bound)
			{
				await this.UnBind();
			}

            if (this._client.Status == ConnectionStatus.Open)
			{
				await this._client.DisconnectAsync();
			}
		}


        private void client_evDisconnected(object sender)
        {
            this._log.Info("SmppClient отключен");

            Sync(this, () =>
            {
                this.bConnect.Enabled = true;
                this.bDisconnect.Enabled = false;
                this.bSubmit.Enabled = false;
                this.cbReconnect.Enabled = true;


            });
        
        }

        private void ClientOnRecoverySucceeded(object sender, BindResp data)
        {
            this._log.Info("Соединение восстановлено.");

            Sync(this, () =>
            {
                this.bConnect.Enabled = false;
                this.bDisconnect.Enabled = true;
                this.bSubmit.Enabled = true;
                this.cbReconnect.Enabled = false;
            });

        }


        private async Task UnBind()
		{
            this._log.Info("Отключение SmppClient");
            UnBindResp resp = await this._client.UnbindAsync();

			switch (resp.Header.Status)
			{
                case CommandStatus.ESME_ROK:
                    this._log.Info("Отключение прошло успешно статус: {0}", resp.Header.Status);
					break;
				default:
                    this._log.Warn("Не удалось выполнить привязку: статус: {0}", resp.Header.Status);
                    await this._client.DisconnectAsync();
					break;
			}

        }



	    private void client_evDeliverSm(object sender, DeliverSm data)
		{
            try
            {
                //Проверьте, получили ли мы ответ о доставке
                if (data.MessageType == MessageTypes.SMSCDeliveryReceipt)
                {
                    //Получить MessageId доставленного сообщения
                    string messageId = data.Receipt.MessageId;
                    MessageState deliveryStatus = data.Receipt.State;

                    this._log.Info("Ответ о доставке получена: {0}", data.Receipt.ToString());
                }
                else
                {

                    // Receive incoming message and try to concatenate all parts
                    if (data.Concatenation != null)
                    {
                        this._messageComposer.AddMessage(data);

                        this._log.Info("Часть ответа смс получена: последовательность: {0}, SourceAddress: {1}, Concatenation ( {2} )" +
                                " Coding: {3}, Text: {4}",
                                data.Header.Sequence, data.SourceAddress, data.Concatenation, data.DataCoding, this._client.EncodingMapper.GetMessageText(data));
                    }
                    else
                    {
                        this._log.Info("Ответ смс получил: последовательность: {0}, SourceAddress: {1}, Coding: {2}, Text: {3}", 
                            data.Header.Sequence, data.SourceAddress, data.DataCoding, this._client.EncodingMapper.GetMessageText(data));
                    }

                    // Проверьте, требуется ли подтверждение ESME
                    if (data.Acknowledgement != SMEAcknowledgement.NotRequested)
                    {
                        // Вы должны уточнить в службе поддержки SMSC, какую информацию они запрашивают в подтверждении ESME.

                        string messageText = data.GetMessageText(this._client.EncodingMapper);

                        var smBuilder = SMS.ForSubmit()
                            .From(data.DestinationAddress)
                            .To(data.SourceAddress)
                            .Coding(data.DataCoding)
                            .Concatenation(ConcatenationType.UDH8bit, this._client.SequenceGenerator.NextReferenceNumber())
                            .Set(m => m.MessageType = MessageTypes.SMEDeliveryAcknowledgement)
                            .Text(new Receipt
                                {
                                    DoneDate = DateTime.Now,
                                    State = MessageState.Delivered,
                                    ErrorCode = "0",
                                    SubmitDate = DateTime.Now,
                                    Text = messageText.Substring(0, Math.Min(20, messageText.Length))
                                }.ToString()
                            );



                        this._client.SubmitAsync(smBuilder).ConfigureAwait(false);
                    }
                }
            } 
            catch (Exception ex)
            {
                data.Response.Header.Status = CommandStatus.ESME_RX_T_APPN;
                this._log.Error(ex, "Не удалось обработать ответ сообщения");
            }
		}


        private void client_evDataSm(object sender, DataSm data)
        {
            this._log.Info("DataSm получил: последовательность: {0}, Адрес источника: {1}, Адрес назначения: {2}, Кодирование: {3}, Текст: {4}",
                data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(this._client.EncodingMapper));
        }



        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            this._log.Info("Получено неполное сообщение От: {0}, Текст: {1}", args.GetFirst<DeliverSm>().SourceAddress, args.Text);
        }

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
	    {
            this._log.Info("Полное сообщение получено от: {0}, To: {1}, Текст: {2}", args.GetFirst<DeliverSm>().SourceAddress, args.GetFirst<DeliverSm>().DestinationAddress, args.Text);
	    }



		private void client_evEnquireLink(object sender, EnquireLink data)
		{
            this._log.Info("Запрошенная ссылка получена");
		}


	

		private void client_evUnBind(object sender, UnBind data)
		{
            this._log.Info("Отменить запрос на получение");
		}

     

 
		private async void bConnect_Click(object sender, EventArgs e)
		{
			await this.Connect();
		}

		private async void bDisconnect_Click(object sender, EventArgs e)
		{
			await this.Disconnect();
		}


		private async void bSubmit_Click(object sender, EventArgs e)
		{

            if (this._client.Status != ConnectionStatus.Bound) 
			{
				MessageBox.Show("Перед отправкой сообщений, пожалуйста, подключитесь к серверу SMPP.");
				return;
			}

            await this.SendSms();
        }

        private async Task SendSms()
        {
            this.bSubmit.Enabled = false;

            this._client.SendSpeedLimit = this.GetSpeedLimit(this.tbSubmitSpeed.Text);

            if (this.cbBatch.Checked)
            {
                await this.SubmitBatchMessages();
            }
            else
            {
                string[] dstAddresses = this.tbDestAdr.Text.Split(',');

                if (dstAddresses.Length == 1)
                {
                    await this.SubmitSingleMessage();
                }
                else if (dstAddresses.Length > 1)
                {
                    await this.SubmitMultiMessage(dstAddresses);
                }
            }

            this.bSubmit.Enabled = true;
        }

        private LimitRate GetSpeedLimit(string text)
        {
            if (string.IsNullOrWhiteSpace(this.tbSubmitSpeed.Text))
            {
                return LimitRate.NoLimit;
            }

            int occurrences;
            if (!int.TryParse(this.tbSubmitSpeed.Text, out occurrences) || occurrences == 0)
                return LimitRate.NoLimit;

            return new LimitRate(occurrences, TimeSpan.FromSeconds(1));

        }

        private async Task SubmitSingleMessage()
	    {
	        DataCodings coding = this.GetDataCoding();



	        var sourceAddress = new SmeAddress(this.tbSrcAdr.Text, (AddressTON)byte.Parse(this.tbSrcAdrTON.Text), (AddressNPI)byte.Parse(this.tbSrcAdrNPI.Text));

	        var destinationAddress = new SmeAddress(this.tbDestAdr.Text, (AddressTON)byte.Parse(this.tbDestAdrTON.Text), (AddressNPI)byte.Parse(this.tbDestAdrNPI.Text));

            this._log.Info("Отправить сообщение: {0}. Текст: {1}", this.tbDestAdr.Text, this.tbMessageText.Text);


            ISubmitSmBuilder builder = SMS.ForSubmit()
	            .From(sourceAddress)
	            .To(destinationAddress)
	            .Coding(coding)
	            .Text(this.tbMessageText.Text)
	            .ExpireIn(TimeSpan.FromDays(2))

                //Запросить ответ о доставке
                .DeliveryReceipt();

	        SubmitMode mode = this.GetSubmitMode();
	        switch (mode)
	        {
	            case SubmitMode.Payload:
	                builder.MessageInPayload();
	                break;
	            case SubmitMode.ShortMessageWithSAR:
	                builder.Concatenation(ConcatenationType.SAR);
	                break;
	        }

            try
            {
                IList<SubmitSmResp> resp = await this._client.SubmitAsync(builder);

                if (resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                {
                    this._log.Info("Отправлено успешно. MessageIds: {0}", string.Join(",", resp.Select(x => x.MessageId)));
                }
                else
                {
                    this._log.Warn("Не удалось отправить. Статус: {0}", string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                }
            }
            catch (Exception ex)
            {
                this._log.Error("Отправить не удалось. Ошибка: {0}", ex.Message);
            }
        }

        private async Task SubmitMultiMessage(string[] dstAddresses)
        {
            DataCodings coding = this.GetDataCoding();

            byte srcTon = byte.Parse(this.tbSrcAdrTON.Text);
            byte srcNpi = byte.Parse(this.tbSrcAdrNPI.Text);
            string srcAdr = this.tbSrcAdr.Text;

            byte dstTon = byte.Parse(this.tbDestAdrTON.Text);
            byte dstNpi = byte.Parse(this.tbDestAdrNPI.Text);

            ISubmitMultiBuilder builder = SMS.ForSubmitMulti()
                .From(srcAdr, (AddressTON)srcTon, (AddressNPI)srcNpi)
                .Coding(coding)
                .Text(this.tbMessageText.Text)
                //Запросить ответ о доставке
                .DeliveryReceipt();

            foreach (var dstAddress in dstAddresses)
            {
                if (dstAddress == null || dstAddress.Trim().Length == 0) continue;

                builder.To(dstAddress.Trim(), (AddressTON)dstTon, (AddressNPI)dstNpi);
            }

            this._log.Info("Отправить сообщение на несколько адресов: {0}. Текст: {1}", string.Join(", ",dstAddresses), this.tbMessageText.Text);


            SubmitMode mode = this.GetSubmitMode();
            switch (mode)
            {
                case SubmitMode.Payload:
                    builder.MessageInPayload();
                    break;
                case SubmitMode.ShortMessageWithSAR:
                    builder.Concatenation(ConcatenationType.SAR);
                    break;
            }

           

            try
            {
                IList<SubmitMultiResp> resp = await this._client.SubmitAsync(builder);

                if (resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                {
                    this._log.Info("Отправлено успешно. MessageIds: {0}", string.Join(",", resp.Select(x => x.MessageId)));
                }
                else
                {
                    this._log.Warn("Не удалось отправить. Status: {0}", string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                }
            }
            catch (Exception ex)
            {
                this._log.Error("Отправить не удалось. Ошибка: {0}", ex.Message);
            }

        }




        private async Task SubmitBatchMessages()
	    {
            var sourceAddress = new SmeAddress(this.tbSrcAdr.Text, (AddressTON)byte.Parse(this.tbSrcAdrTON.Text), (AddressNPI)byte.Parse(this.tbSrcAdrNPI.Text));

	        var destinationAddress = new SmeAddress(this.tbDestAdr.Text, (AddressTON)byte.Parse(this.tbDestAdrTON.Text), (AddressNPI)byte.Parse(this.tbDestAdrNPI.Text));


	        string messageText = this.tbMessageText.Text;

	        SubmitMode mode = this.GetSubmitMode();

	        DataCodings coding = this.GetDataCoding();

	        int count = int.Parse(this.tbRepeatTimes.Text);

            this._log.Info("Отправить сообщение партией. Количество: {0}. Текст: {1}", count, messageText);

	        // bulk sms test
	        List<SubmitSm> batch = new List<SubmitSm>();
	        for (int i = 0; i < count; i++)
	        {
	            ISubmitSmBuilder builder = SMS.ForSubmit()
	                .Text(messageText)
	                .From(sourceAddress)
	                .To(destinationAddress)
	                .Coding(coding);

	            switch (mode)
	            {
	                case SubmitMode.Payload:
	                    builder.MessageInPayload();
	                    break;
	                case SubmitMode.ShortMessageWithSAR:
	                    builder.Concatenation(ConcatenationType.SAR);
	                    break;
	            }

                batch.AddRange(builder.Create(this._client));

	        }


         

            try
            {
                Stopwatch watch = Stopwatch.StartNew();

                var resp = (await this._client.SubmitAsync(batch)).ToArray();

                watch.Stop();

                if (resp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                {
                    this._log.Info("Пакетная отправка завершена. Отправлено: {0}, Прошедшее: {1} ms, Представление: {2} m/s", 
                        batch.Count, watch.ElapsedMilliseconds, batch.Count * 1000f / watch.ElapsedMilliseconds);
                }
                else
                {
                    var wrongStatuses = resp.Where(x => x.Header.Status != CommandStatus.ESME_ROK)
                        .Select(x => x.Header.Status).Distinct();

                    this._log.Warn("Отправить не удалось. Неверный статус: {0}", string.Join(", ", wrongStatuses));
                }
            }
            catch (Exception ex)
            {
                this._log.Error("Отправить не удалось. Ошибка: {0}", ex.Message);
            }
         }

	    private SubmitMode GetSubmitMode()
        {
            return (SubmitMode)Enum.Parse(typeof(SubmitMode), this.cbSubmitMode.Text);
        }

        private DataCodings GetDataCoding()
        {
            return (DataCodings)Enum.Parse(typeof(DataCodings), this.cbDataCoding.Text);
        }

        private void SmppClientDemo_Load(object sender, EventArgs e)
        {
            this.cbSubmitMode.SelectedIndex = 1;
            this.cbDataCoding.SelectedIndex = 0;

            this.cbBindingMode.Items.Clear();
            foreach (ConnectionMode mode in Enum.GetValues(typeof(ConnectionMode)))
            {
                if (mode == ConnectionMode.None) continue;
                this.cbBindingMode.Items.Add(mode);
            }
            this.cbBindingMode.SelectedItem = ConnectionMode.Transceiver;
        }

       



        private void cbAsync_CheckedChanged(object sender, EventArgs e)
        {
            this.tbRepeatTimes.Enabled = this.cbBatch.Checked;
        }



        private void bAbout_Click(object sender, EventArgs e)
        {
            AboutSmppClientDemo frm = new AboutSmppClientDemo();
            frm.ShowDialog();
        }

        private void SmppClientDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._client.evDisconnected -= this.client_evDisconnected;
            this._client.Dispose();
        }


        public delegate void SyncAction();

        public static void Sync(Control control, SyncAction action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action, new object[] { });
                return;
            }

            action();
        }

        private void cbReconnect_CheckedChanged(object sender, EventArgs e)
        {
            this._client.ConnectionRecovery = this.cbReconnect.Checked;   
        }
    }
  }