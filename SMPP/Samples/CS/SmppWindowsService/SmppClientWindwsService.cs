using System.ServiceProcess;
using System.Threading.Tasks;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;

namespace SmppWindowsService
{
    public class SmppClientWindwsService : ServiceBase
    {
        private SmppClientSettings _settings;

        private readonly SmppClient _client;
        private readonly ILog _log;
 
        public SmppClientWindwsService()
        {
            ServiceName = "SMPPClient";

            LogManager.SetLoggerFactory(new FileLogFactory(ServiceName+".log", LogLevel.Info));

            _log = LogManager.GetLogger(GetType().Name);

            _client = new SmppClient();
            _client.ConnectionRecovery = true;
            _client.evDeliverSm += ClientOnDeliverSm;
        }

   
        private void ClientOnDeliverSm(object sender, DeliverSm data)
        {
           
        }

        protected override void OnStart(string[] args)
        {
            _settings = LoadSettings();

            Task connectTask = ConnectToSmppServerAsync();
        }

        private async Task ConnectToSmppServerAsync()
        {
            if (await _client.ConnectAsync(_settings.ServerAddress, _settings.ServerPort))
            {
                BindResp bindResp = await _client.BindAsync(_settings.Username, _settings.Password);
                if (bindResp.Header.Status == CommandStatus.ESME_ROK)
                {
                    // start some sending
                }
            }
        }

        private SmppClientSettings LoadSettings()
        {
            return new SmppClientSettings
            {
                //some settings from any source. f.i. config file
            };
        }

        protected override void OnStop()
        {
            _log.Info("Client disconnected from {0}",  _client.RemoteEndPoint);

            _client.DisconnectAsync();

        }


    }

    internal class SmppClientSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
    }
}