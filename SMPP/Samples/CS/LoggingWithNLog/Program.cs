using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;
using NLog.Config;
using NLog.Targets;

namespace LoggingWithNLog
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingConfiguration configuration = new LoggingConfiguration();
            var consoleTarget = new ConsoleTarget("console");

            configuration.AddTarget(consoleTarget);
            configuration.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, consoleTarget);
          
            NLog.LogManager.Configuration = configuration;

            LogManager.SetLoggerFactory(new NLogLoggerFactory());


            SendHelloWorld().Wait();

            Console.ReadLine();
        }

        // <Послать HelloWorld>
        public static async Task SendHelloWorld()
        {

            using (SmppServer server = new SmppServer(new IPEndPoint(IPAddress.Any, 7777)))
            {
                server.Start();

                using (SmppClient client = new SmppClient())
                {

                    if (await client.ConnectAsync("localhost", 7777))
                    {
                        BindResp bindResp = await client.BindAsync("1", "2");

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
        }
        //</Послать HelloWorld>
    }
}

