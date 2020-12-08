using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.SetLoggerFactory(new ConsoleLogFactory(LogLevel.Verbose));


            SendHelloWorld().Wait();
        }

        // <Послать HelloWorld>
        public static async Task SendHelloWorld()
        {
            using (SmppClient client = new SmppClient())
            {

                if (await client.ConnectAsync("smpp.server.address", 7777))
                {
                    BindResp bindResp = await client.BindAsync("username", "password");

                    if (bindResp.Header.Status == CommandStatus.ESME_ROK)
                    {
                        var submitResp = await client.SubmitAsync(
                            SMS.ForSubmit()
                                .From("111")
                                .To("222")
                                .Coding(DataCodings.UCS2)
                                .Text("Hello World!"));

                        if (submitResp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                        {
                            client.Logger.Info("Сообщение было отправлено.");
                        }
                    }

                    await client.DisconnectAsync();
                }
            }
        }
        //</Послать HelloWorld>
    }
}
