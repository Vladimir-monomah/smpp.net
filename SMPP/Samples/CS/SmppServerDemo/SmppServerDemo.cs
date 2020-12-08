using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;
using SmppLogger;

namespace SmppServerDemo
{
    public partial class SmppServerDemo : Form
    {
        private SmppServer _server;
        private readonly ILog _log;
        private readonly MessageComposer _messageComposer;



        public SmppServerDemo()
        {
            this.InitializeComponent();


            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogServer.txt");
            LogManager.SetLoggerFactory(new TextBoxLogFactory(this.tbLog, LogLevel.Info, fileName));
           


            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogManager.GetLogger("AppDomain").Fatal((Exception)args.ExceptionObject, "Unhandled Exception");
            };

            this._log = LogManager.GetLogger(this.GetType().Name);

            //Create message composer. It helps to get full text of the concatenated message in the method OnFullMessageReceived
            this._messageComposer = new MessageComposer();
            this._messageComposer.evFullMessageReceived += this.OnFullMessageReceived;
            this._messageComposer.evFullMessageTimeout += this.OnFullMessageTimeout;
        }

        

        private void ServerOnClientEnquireLink(object sender, SmppServerClient client, EnquireLink data)
        {
            this._log.Info($"EnquireLink received from {client}");
        }

        private void OnClientCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }

        private long messageIdCounter = 0;

        private void server_evClientBind(object sender, SmppServerClient client, Bind data)
        {
            this._log.Info("Client {0} bind as {1}:{2}", client.RemoteEndPoint, data.SystemId, data.Password);

            //  data.Response.ChangeSystemId("NewServerId");

            //Check SMPP access, and if it is wrong retund non-OK status.
            if (data.SystemId == "")
            {
                data.Response.Header.Status = CommandStatus.ESME_RINVSYSID;
                this._log.Info("Client {0} tries to bind with invalid SystemId: {1}", client.RemoteEndPoint, data.SystemId);
                return;
            }
            if (data.Password == "")
            {
                this._log.Info(string.Format("Client {0} tries to bind with invalid Password.", client.RemoteEndPoint));

                data.Response.Header.Status = CommandStatus.ESME_RINVPASWD;
                return;
            }

            //deny multiple connection with same smpp system id.
            foreach (var connectedClient in this._server.ConnectedClients)
            {
                if (connectedClient.SystemID == client.SystemID && connectedClient.Status == ConnectionStatus.Bound)
                {
                    data.Response.Header.Status = CommandStatus.ESME_RALYBND;
                    this._log.Warn("Client {0} tries to establish multiple sessions with the same SystemId", client.RemoteEndPoint);
                    return;
                }
            }

            this._log.Info("Client {0} has been bound.", client.RemoteEndPoint);
            //  CommandStatus.ESME_RBINDFAIL - when Bind Failed. 

            this.UpdateClient(client);
        }

        private void server_evClientUnbind(object sender, SmppServerClient client, UnBind data)
        {
        }

        private void server_evClientSubmitSm(object sender, SmppServerClient client, SubmitSm data)
        {
            long messageId = Interlocked.Increment(ref this.messageIdCounter);
            // You can set your own MessageId
            data.Response.MessageId = messageId.ToString();

            this._log.Info("Client {0} sends message From:{1}, To:{2}, Text: {3}",
                client.RemoteEndPoint,  data.SourceAddress, data.DestinationAddress,
                data.GetMessageText(client.EncodingMapper));


            this._messageComposer.AddMessage(data);

           

            // Set unsuccess response status
            //data.Response.Status = CommandStatus.ESME_RSUBMITFAIL;


            if (data.SMSCReceipt != SMSCDeliveryReceipt.NotRequested)
            {
                //Send Delivery Receipt when required

                string messageText = data.GetMessageText(client.EncodingMapper);

                var dlrBuilder = SMS.ForDeliver()
                    .From(data.DestinationAddress)
                    .To(data.SourceAddress)
                    .Receipt(new Receipt
                    {
                        DoneDate = DateTime.Now,
                        State = MessageState.Delivered,
                        MessageId = data.Response.MessageId,
                        ErrorCode = "0",
                        SubmitDate = DateTime.Now,
                        Text = messageText.Substring(0, Math.Min(20, messageText.Length))
                    });

                if (data.DataCoding == DataCodings.UCS2)
                {
                    //short_message field cannot contain user data longer than 255 octets,
                    //therefore for UCS2 encoding we are sending DLR in message_payload parameter
                    dlrBuilder.MessageInPayload();
                }

               client.DeliverAsync(dlrBuilder).ConfigureAwait(false);
            }

            

        }

        void server_evClientSubmitMulti(object sender, SmppServerClient client, SubmitMulti data)
        {

            this._log.Info("Client {0} sends message From:{1} to multiple destinations:{2}, Text: {3}",
                                       client.RemoteEndPoint, data.SourceAddress, data.DestinationAddresses.Count, 
                                       data.GetMessageText(client.EncodingMapper));

            this._messageComposer.AddMessage(data);

            if (data.RegisteredDelivery == 1)
            {
                SmeAddress destinationAddress = data.DestinationAddresses[0] as SmeAddress;

                string messageText = data.GetMessageText(client.EncodingMapper);

                //Send Delivery Receipt when required
                Task.Run(()=> client.DeliverAsync(
                    SMS.ForDeliver()
                        .From(data.SourceAddress)
                        .To(destinationAddress)
                        .Coding(data.DataCoding)
                        .Receipt(new Receipt
                            {
                                DoneDate = DateTime.Now,
                                State = MessageState.Delivered,
                                MessageId = data.Response.MessageId,
                                ErrorCode = "0",
                                SubmitDate = DateTime.Now,
                                Text = messageText.Substring(0, Math.Max(20, messageText.Length))
                            }
                        )
                ));
            }

         

        }

     

        private void UpdateClient(SmppServerClient client)
        {
            Sync(this, () =>
            {
                int index = -1;
                for (int i = 0; i < this.lbClients.Items.Count; i++)
                {
                    SmppServerClient item = this.lbClients.Items[i] as SmppServerClient;
                    if (item != null && item.RemoteEndPoint.Equals(client.RemoteEndPoint))
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0)
                {
                    this.lbClients.Items[index] = client;
                }
                else
                {
                    this.lbClients.Items.Add(client);
                }
            });
        }


        private void bStartServer_Click(object sender, EventArgs e)
        {
            this.bStartServer.Enabled = false;
            this.bStopServer.Enabled = true;

            this._server?.Stop();
            this._server?.Dispose();
            this._server = new SmppServer(new IPEndPoint(IPAddress.Any, int.Parse(this.tbPort.Text)));
            this._server.evClientConnected += this.server_evClientConnected;
            this._server.evClientDisconnected += this.server_evClientDisconnected;
            this._server.evClientBind += this.server_evClientBind;
            this._server.evClientUnBind += this.server_evClientUnbind;
            this._server.evClientSubmitSm += this.server_evClientSubmitSm;
            this._server.evClientSubmitMulti += this.server_evClientSubmitMulti;
            this._server.evClientEnquireLink += this.ServerOnClientEnquireLink;

            this._server.evClientCertificateValidation += this.OnClientCertificateValidation;

            if (this.cbSSL.Checked && this.comboCertList.SelectedItem != null)
            {
                this._server.ServerCertificate = (X509Certificate2)this.comboCertList.SelectedItem;
                this._server.EnabledSslProtocols = SslProtocols.Default;
            }
            else
            {
                this._server.ServerCertificate = null;
            }
            this._server.Start();
        }

        private void bStopServer_Click(object sender, EventArgs e)
        {
            this._server?.Stop();

            this.bStartServer.Enabled = true;
            this.bStopServer.Enabled = false;

            this.lbClients.Items.Clear();
           
        }

        private void SmppServerDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.bStopServer_Click(sender, EventArgs.Empty);
        }


        void server_evClientConnected(object sender, SmppServerClient client)
        {
            //Change number of threads that process received messages. Dafault is 3
            //client.WorkerThreads = 10;

            //Change receive buffer size for client socket
            // client.ReceiveBufferSize = 30 * 1024 * 1024;
            //Change send buffer size for client socket
            //  client.SendBufferSize = 30 * 1024 * 1024;


            //Don't allow this client to send more than one message per second
            //client.ReceiveSpeedLimit = 1;
            //Set maximum number of unhandled messages in the receive queue for this client
            //client.ReceiveQueueLimit = 2;


            client.EncodingMapper.MapEncoding(DataCodings.Class1, new Inetlab.SMPP.Encodings.GSMPackedEncoding());


            this._log.Info("Client {0} connected.", client.RemoteEndPoint);


            if (client.ClientCertificate != null)
            {
                this._log.Info("Client Certificate {0}, Expire Date: {1}", client.ClientCertificate.Subject, client.ClientCertificate.GetExpirationDateString());
            }

            Sync(this.lbClients, () =>
            {
                this.lbClients.Items.Add(client);
            });
        }

       

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
        {
            this._log.Info("SMS Received: {0}", args.Text);
          
        }

        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            this._log.Info("Incomplete SMS Received: {0}", args.Text);
        }



        private void server_evClientDisconnected(object sender, SmppServerClient client)
        {

            this._log.Info("Client {0} disconnected.", client.RemoteEndPoint);

            Sync(this.lbClients, () =>
            {
                this.lbClients.Items.Remove(client);
            });

        }


        private void cbSSL_CheckedChanged(object sender, EventArgs e)
        {
            this.comboCertList.Enabled = this.cbSSL.Checked;
            this.comboCertList.Items.Clear();

            if (this.cbSSL.Checked)
            {
                X509CertificateCollection collection = new X509Certificate2Collection();


                if (File.Exists("server.p12"))
                {
                    collection.Add(new X509Certificate2("server.p12", "12345"));
                }

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                collection.AddRange(store.Certificates);
  
              
                foreach (X509Certificate x509 in collection)
                {
                    this.comboCertList.Items.Add(x509);
                }

                if (this.comboCertList.Items.Count > 0) this.comboCertList.SelectedIndex = 0;

            }

        }



        private void lbClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSelected = this.lbClients.SelectedIndex >= 0;

            this.bSendMessage.Enabled = isSelected;
            this.bDisconnect.Enabled = isSelected;
        }

        private void bSendMessage_Click(object sender, EventArgs e)
        {
            SmppServerClient client = this.lbClients.SelectedItem as SmppServerClient;
            if (client != null)
            {
                SendMessage form = new SendMessage(client);
                form.ShowDialog();
            }
        }

        private async void bDisconnect_Click(object sender, EventArgs e)
        {
            SmppServerClient client = this.lbClients.SelectedItem as SmppServerClient;
            if (client != null)
            {
                if (client.Status == ConnectionStatus.Bound)
                {
                  await client.UnbindAsync();
                }

                if (client.Status == ConnectionStatus.Open)
                {
                  await client.DisconnectAsync();
                }
            }
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
    }
}