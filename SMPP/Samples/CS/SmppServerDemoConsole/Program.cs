using System;
using System.Net;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;

namespace SmppServerDemo
{
    class Program
    {
        private SmppServer _server;

        static void Main(string[] args)
        {
            LogManager.SetLoggerFactory(new ConsoleLogFactory(LogLevel.Info));

            Console.Title = "SmppServer Demo";

            Program p = new Program();
            p.Run();
            Console.ReadLine();
        }

        private void Run()
        {
            _server = new SmppServer(new IPEndPoint(IPAddress.Any, 7777));


            _server.evClientConnected += (sender, client) =>
            {

            };
            _server.evClientDisconnected += (sender, client) =>
            {

            };
            _server.evClientSubmitSm += WhenServerReceivesPDU;
            _server.Start();
        }

        private async void WhenServerReceivesPDU(object sender, SmppServerClient serverClient, SubmitSm data)
        {


            if (data.RegisteredDelivery == 1)
            {

                //Отправить ответ о доставке при необходимости
                await serverClient.DeliverAsync(
                    SMS.ForDeliver()
                        .From(data.SourceAddress)
                        .To(data.DestinationAddress)
                        .Coding(data.DataCoding)
                        .Receipt(new Receipt
                            {
                                DoneDate = DateTime.Now,
                                State = MessageState.Delivered,
                                MessageId = data.Response.MessageId,
                                ErrorCode = "0",
                                SubmitDate = DateTime.Now
                            }
                        )
                )
                .ConfigureAwait(false);
            }
        }

    }
}
