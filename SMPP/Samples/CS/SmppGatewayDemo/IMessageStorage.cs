using System;
using Inetlab.SMPP.PDU;

namespace SmppGatewayDemo
{
    internal interface IMessageStorage
    {
        void SubmitReceived(SubmitSm data);
        void SubmitForwarded(SubmitSm req, SubmitSmResp remoteResp);

        void ReceiptDelivered(string localMessageId);
        void DeliveryReceiptNotRequested(string localMessageId);

        void ReceiptReceived(DeliverSm data);

        event EventHandler<DeliverSm> ReceiptReadyForForward;
        
    }
}