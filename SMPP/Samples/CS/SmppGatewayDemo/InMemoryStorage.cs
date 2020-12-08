using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Parameters;
using Inetlab.SMPP.PDU;

namespace SmppGatewayDemo
{
    internal class InMemoryStorage: IMessageStorage
    {
        class MessageEntry
        {
            public uint LocalSequence;
            public string LocalMessageId;
            public uint RemoteSequence;
            public string RemoteMessageId;
            public DeliverSm Receipt { get; set; }
        }

        readonly ConcurrentDictionary<uint, MessageEntry> _localSequences = new ConcurrentDictionary<uint, MessageEntry>();
        readonly ConcurrentDictionary<string, MessageEntry> _remoteMessage = new ConcurrentDictionary<string, MessageEntry>();
        readonly ConcurrentDictionary<string, MessageEntry> _localMessage = new ConcurrentDictionary<string, MessageEntry>();


        public void SubmitReceived(SubmitSm data)
        {

            var entry = new MessageEntry
            {
                LocalSequence = data.Header.Sequence,
                LocalMessageId = data.Response.MessageId
            };

            if (!_localSequences.TryAdd(data.Header.Sequence, entry))
            {
                throw new Exception("Невозможно добавить сообщение с той же локальной последовательностью " + data.Header.Sequence);
            }

            if (!_localMessage.TryAdd(data.Response.MessageId, entry))
            {
                throw new Exception("Невозможно добавить сообщение с тем же локальным идентификатором сообщения " + data.Response.MessageId);
            }

        }

        public void SubmitForwarded(SubmitSm req, SubmitSmResp remoteResp)
        {
            MessageEntry entry;
            if (!_localSequences.TryGetValue(req.Header.Sequence, out entry))
            {
                throw new Exception("Не удается найти сообщение с локальной последовательностью " + req.Header.Sequence);
            }

            entry.RemoteSequence = remoteResp.Header.Sequence;
            entry.RemoteMessageId = remoteResp.MessageId;

            MessageEntry remoteEntry;
            if (_remoteMessage.TryRemove(remoteResp.MessageId, out remoteEntry))
            {
                entry.Receipt = remoteEntry.Receipt;
                OnReceiptReadyForForward(entry);
            }
            else
            {
                _remoteMessage.TryAdd(remoteResp.MessageId, entry);
            }
        }

      


        public void ReceiptDelivered(string localMessageId)
        {
            MessageEntry entry;
            if (_localMessage.TryRemove(localMessageId, out entry))
            {
                if (entry.RemoteMessageId != null)
                {
                    MessageEntry entryRemote;
                    _remoteMessage.TryRemove(entry.RemoteMessageId, out entryRemote);
                }

                MessageEntry entryLocal;
                if (_localSequences.TryRemove(entry.LocalSequence, out entryLocal))
                {

                }
            }
        }

        public void DeliveryReceiptNotRequested(string localMessageId)
        {
            MessageEntry entry;
            if (_localMessage.Remove(localMessageId, out entry))
            {
                if (entry.RemoteMessageId != null)
                {
                    MessageEntry entryRemote;
                    _remoteMessage.TryRemove(entry.RemoteMessageId, out entryRemote);
                }

                MessageEntry entryLocal;
                if (_localSequences.TryRemove(entry.LocalSequence, out entryLocal))
                {

                }
            }
        }

        public void ReceiptReceived(DeliverSm data)
        {
            MessageEntry entry;

            if (_remoteMessage.TryGetValue(data.Receipt.MessageId, out entry))
            {
                entry.Receipt = data;

                OnReceiptReadyForForward(entry);
            }
            else
            {
                _remoteMessage.TryAdd(data.Receipt.MessageId, new MessageEntry
                {
                    RemoteMessageId = data.Receipt.MessageId,
                    Receipt = data
                });
            }
        }

        private void OnReceiptReadyForForward(MessageEntry entry)
        {
            var handler = ReceiptReadyForForward;
            if (handler != null)
            {
                DeliverSm receipt = entry.Receipt.ClonePDU();
                receipt.Receipt.MessageId = entry.LocalMessageId;

                receipt.Parameters[OptionalTags.ReceiptedMessageId] =
                    EncodingMapper.Default.GetMessageBytes(receipt.Receipt.MessageId, receipt.DataCoding);

                receipt.Header.Sequence = 0;

                handler(this, receipt);
            }
        }

        public event EventHandler<DeliverSm> ReceiptReadyForForward;
    }
}