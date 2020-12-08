using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogLevel = Inetlab.SMPP.Logging.LogLevel;

namespace LoggingWithMicrosoft
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();

            //Включить ведение журнала отладки для библиотеки SMPP
            Inetlab.SMPP.Logging.LogManager.SetLoggerFactory(new LoggerFactoryAdapter(loggerFactory, LogLevel.Debug));

            return host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HelloWorldService>();
                });


   }

    public class HelloWorldService : IHostedService
    {
        private readonly SmppServer _server;
        private readonly SmppClient _client;

        public HelloWorldService()
        {
            _server = new SmppServer(new IPEndPoint(IPAddress.Any, 7777));
            _server.evClientBind += ServerOnClientBind;
            _server.evClientSubmitSm += ServerOnClientSubmitSm;


            _client = new SmppClient();
        }

        private void ServerOnClientSubmitSm(object sender, SmppServerClient client, SubmitSm data)
        {
        }

        private void ServerOnClientBind(object sender, SmppServerClient client, Bind data)
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _server.Start();

            Task.Run(Run);

            return Task.CompletedTask;
            
        }

        private async Task Run()
        {
            if (await _client.ConnectAsync("localhost", 7777))
            {
                BindResp bindResp = await _client.BindAsync("1", "2");

                if (bindResp.Header.Status == CommandStatus.ESME_ROK)
                {
                    var submitResp = await _client.SubmitAsync(
                        SMS.ForSubmit()
                            .From("111")
                            .To("222")
                            .Coding(DataCodings.UCS2)
                            .Text("Hello World!"));

                    if (submitResp.All(x => x.Header.Status == CommandStatus.ESME_ROK))
                    {
                        _client.Logger.Info("СМС-СООБЩЕНИЕ ОТПРАВЛЕНО!");
                    }
                }

              
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           _client.Dispose();
           _server.Dispose();

           return Task.CompletedTask;
           
        }
    }
}
