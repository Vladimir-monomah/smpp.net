using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;

namespace SmppGatewayDemo
{
    class Program
    {
        private SmppClient _proxyClient;
        private SmppServer _proxyServer;

        private readonly IMessageStorage _messageStorage = new InMemoryStorage();
      

        static void Main(string[] args)
        {
            LogManager.SetLoggerFactory(new ConsoleLogFactory(LogLevel.Info));

            Console.Title = "Тестовый шлюз";

            Uri url = null;
            if (args.Length > 0)
            {
                Uri.TryCreate(args[0], UriKind.Absolute, out url);
            }

            if (url == null)
            {
                url = new Uri("tcp://localhost:7777");
            }

            Program p = new Program();
            p.Run(url).ConfigureAwait(false);

            Console.ReadLine();
        }

        private async Task Run(Uri url)
        {


            _proxyServer = new SmppServer(new IPEndPoint(IPAddress.Any, 7776));
            _proxyServer.Name = "Прокси" + _proxyServer.Name;
 
            _proxyServer.evClientBind += (sender, client, data) => { };

            _proxyServer.evClientSubmitSm += async (sender, serverClient, data) =>
            {
                try
                {
                    _messageStorage.SubmitReceived(data);

                    SubmitSm submitSm = data.ClonePDU();

                    submitSm.Header.Sequence = 0;

                    SubmitSmResp resp = await _proxyClient.SubmitAsync(submitSm);

                    _messageStorage.SubmitForwarded(data, resp);

                    if (data.SMSCReceipt == SMSCDeliveryReceipt.NotRequested)
                    {
                        _messageStorage.DeliveryReceiptNotRequested(data.Response.MessageId);
                    }
                }
                catch (Exception ex)
                {
                    serverClient.Logger.Error(ex, "Не удалось обработать SubmitSm.");
                }

            };

            _proxyServer.evClientConnected += (sender, client) =>
            {
                client.WorkerThreads = 10;
            };
            _proxyServer.evClientDisconnected += (sender, client) =>
            {
            };

            _proxyServer.Start();

           
            _proxyClient = new SmppClient();
            _proxyClient.WorkerThreads = 10;
            _proxyClient.Name = "Прокси" + _proxyClient.Name;

            _proxyClient.evDeliverSm += (sender, data) =>
            {

                var client = _proxyServer.ConnectedClients.FirstOrDefault();
                if (client != null)
                {

                    if (data.MessageType == MessageTypes.SMSCDeliveryReceipt)
                    { 
                        _messageStorage.ReceiptReceived(data);
                    }
                }
            };


            _messageStorage.ReceiptReadyForForward += async (sender, data) =>
            {
                var client = _proxyServer.ConnectedClients.FirstOrDefault();
                if (client != null)
                {

                    DeliverSmResp resp = await client.DeliverAsync(data);

                    _messageStorage.ReceiptDelivered(data.Receipt.MessageId);

                }

            };

            await _proxyClient.ConnectAsync(url.Host, url.Port);

            BindResp bindResp = await _proxyClient.BindAsync("прокси", "тест");
        }
    }
}
