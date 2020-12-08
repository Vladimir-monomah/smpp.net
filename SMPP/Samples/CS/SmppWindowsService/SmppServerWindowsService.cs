using System;
using System.Net;
using System.ServiceProcess;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Encodings;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;

namespace SmppWindowsService
{
    public class SmppServerWindowsService : ServiceBase
    {
        private readonly SmppServer _server;
        private readonly ILog _log;
      
        public SmppServerWindowsService()
        {
            ServiceName = "SMPPServer";

          
            LogManager.SetLoggerFactory( new FileLogFactory(ServiceName+".log", LogLevel.Info));

            _log = LogManager.GetLogger(GetType().Name);

             _server = new SmppServer(new IPEndPoint(IPAddress.Any, 7777));
             _server.evClientConnected += server_evClientConnected;
             _server.evClientDisconnected += server_evClientDisconnected;
             _server.evClientBind += server_evClientBind;
             _server.evClientSubmitSm += server_evClientSubmitSm;
        }

        protected override void OnStart(string[] args)
        {
            _server.Start();
        }

        protected override void OnStop()
        {
            _server.Stop();
        }


        void server_evClientConnected(object sender, SmppServerClient client)
        {
            _log.Info("Client {0} connected.", client.RemoteEndPoint);

            //If needed, we can change encoding mapping for the client
            client.EncodingMapper.MapEncoding(DataCodings.OctetUnspecified, new GSMEncoding());
        }

        void server_evClientDisconnected(object sender, SmppServerClient client)
        {
            _log.Info("Client {0} disconnected.", client.RemoteEndPoint);
        }

        private void server_evClientBind(object sender, SmppServerClient client, Inetlab.SMPP.PDU.Bind data)
        {
            _log.Info("Client {0} Binding {1}:{2}", client.RemoteEndPoint, data.SystemId, data.Password);

            //  data.Response.ChangeSystemId("NewServerId");

            //Check SMPP access, and if it is wrong returns non-OK status.
            if (data.SystemId == "")
            {
                data.Response.Header.Status = CommandStatus.ESME_RINVSYSID;
                return;
            }
            if (data.Password == "")
            {
                data.Response.Header.Status = CommandStatus.ESME_RINVPASWD;
                return;
            }

            //  CommandStatus.ESME_RALYBND  - when SMPP account already connected.
            //  CommandStatus.ESME_RBINDFAIL - when Bind Failed. 

        }

        void server_evClientSubmitSm(object sender, SmppServerClient client, Inetlab.SMPP.PDU.SubmitSm data)
        {
            string messageText = data.GetMessageText(client.EncodingMapper);


            _log.Info(string.Format("Client {0} Received message Src:{1}, Dst:{2}, Text: {3}",
                client.RemoteEndPoint, data.SourceAddress, data.DestinationAddress,
                messageText));
            

            // You can set your own MessageId
            // data.Response.MessageId = "11";

            //if (data.RegisteredDelivery == 1)

            //    //Send Delivery Receipt when required
            //    client.DeliverAsync(
            //        SMS.ForDeliver()
            //            .From(data.SourceAddress)
            //            .To(data.DestinationAddress)
            //            .Coding(data.DataCoding)
            //            .Receipt(new Receipt
            //                {
            //                    DoneDate = DateTime.Now,
            //                    State = MessageState.Delivered,
            //                    MessageId = data.Response.MessageId,
            //                    ErrorCode = "0",
            //                    SubmitDate = DateTime.Now,
            //                    Text = messageText
            //                }
            //            )
            //    );

        }
    }

  
  
}
