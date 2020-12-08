using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;

namespace TestLocalPerformance
{
    class Program
    {
        static void Main(string[] args)
        {

            LogManager.SetLoggerFactory(new ConsoleLogFactory(LogLevel.Info));

            StartApp().ConfigureAwait(false);

            Console.ReadLine();
        }

        public static async Task StartApp()
        {

            using (SmppServer server = new SmppServer(new IPEndPoint(IPAddress.Any, 7777)))
            {
                server.evClientBind += (sender, client, data) => { /*accept all*/ };
                server.evClientSubmitSm += (sender, client, data) => {/*receive all*/ };
                server.Start();

                using (SmppClient client = new SmppClient())
                {


                    await client.ConnectAsync("localhost", 7777);

                    await client.BindAsync("username", "password");

                    int batchSize = 50000;

                    Console.WriteLine($"Послать {batchSize} сообщения.");

                    client.Metrics.Reset();

                    var batch = CreateBatch(client.EncodingMapper, batchSize);

                    Stopwatch watch = Stopwatch.StartNew();

                    await client.SubmitAsync(batch);

                    watch.Stop();

                    var clientMetrics = client.Metrics.Snapshot();

                    Console.WriteLine($"Прошедшие: {watch.ElapsedMilliseconds} секунды");

                    Console.WriteLine($"Представление: {clientMetrics.Sent.Requests.AvgPerSecond} мин/сек");
                    
                }
            }
        }


        public static IReadOnlyList<SubmitSm> CreateBatch(EncodingMapper encodingMapper, int messagesCount)
        {
            List<SubmitSm> requests = new List<SubmitSm>();

            for (int i = 0; i < messagesCount; i++)
            {
                requests.AddRange(SMS.ForSubmit().From("1111").To("2222").Text("test" + (i + 1)).Create(encodingMapper));
            }
            return requests;
        }
    }
}
