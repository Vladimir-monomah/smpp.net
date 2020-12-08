# Changelog 

## [2.8.1] - 2020-06-16
### Fixed
- SendSpeedLimit and result speed has significant difference.
- Send Queue can be blocked in some edge cases.
- Reconnect doesn't start when ThreadPool is overloaded
- Race condition in MessageComposer causes NullReferenceException

## [2.8.0] - 2020-04-02
### Added:
- SendResponseAsync method in SmppClientBase class. Response sending can be prevented in a event handler by changing it to null. req.Response = null; 
- Extension method CanBeEncoded to validate an Encoding for given text message
- MessageComposer with persistence storage interface to save message parts in external database instead of memory.

### Fixed:
- NullReferenceException in the event evFullMessageReceived of MessageComposer class
- Setup Project: Unable to update the dependencies of the project.  The dependencies for the object 'Inetlab.SMPP.dll' cannot be determined.

## [2.7.1] - 2020-01-20
### Changed
- move InterfaceVersion property to the SmppClientBase class 

### Fixed:
- GenericNack PDU has not been sent when wrong PDU header is received. 


## [2.7.0] - 2019-12-11
### Added
- Added Metrics property for SmppClientBase class.
- Support of 16 bit concatenation parameters in SMS builder classes.

### Changed
- ReceiveSpeedLimit with rate limiting. Measure PDU count for defined time unit instead of interval between PDUs. 
- rename async-methods according to the dotnet naming conventions
- InactivityTimeout starts when SmppServerClient is connected and EnquireLinkInterval is not defined for this client.
- Generate long number MessageId for SubmitSmResp and SubmitMultiResp. According to SMPP Protocol MessageId should contain only digits.
- Timeout timer in MessageComposer restarts when next segment of the message is received.

### Fixed
- Submit hangs after unexpected disconnect.
- Exception by changing send or receive buffer size.
- SmppTime.Format for relative time.
- OverflowException in GSMEncoding.

### Removed
- Support of .NET Standard 1.4
- InactivityTimeout from SmppClient


## [2.6.14] - 2019-09-20
### Fixed
- SmppTime.Format for relative time.
- exception in demo applications


## [2.6.13] - 2019-08-14
### Fixed
- multithread-issue with ConnectedClients in SmppServer class
- set SmppClient.SystemID and SmppClient.SystemType properties when client is bound.

## [2.6.12] - 2019-07-26
### Added
- convert UserDataHeader to and from byte array
- SmppTime functions for formatting and parsing scheduled delivery times and expiry times in PDU.
- EnsureReferenceNumber method that sets next reference number for a list of concatenated PDUs.
- property InactivityTimeout in the class SmppClientBase. Default is 2 minutes. Connection will be dropped when in specified period of time no SMPP message was exchanged.
           InactivityTimeout doesn't work when EnquireLinkInterval is defined.

### Fixed
- SmppServer: when client.ReceiveSpeedLimit is set to any value, first message is always throttled.
- text splitting: Incorrect message length of 1st PDU when text encoded in GSM encoding and contains extended characters
- ReferenceNumber=0 for submitted concatenated PDUs. 

## [2.6.11] - 2019-04-20
### Fixed
- Connection failed. Error Code: 10048. Only one usage of each socket address (protocol/network address/port) is normally permitted. Occurs when call Connect method from different threads at the same time.
 
## [2.6.10] - 2019-04-19
### Fixed
- exceptions by incorrect disconnect.

## [2.6.9] - 2019-04-15
### Fixed
- Request property is null in received response PDU class.

### Added
- ReceiveBufferSize and SendBufferSize properties for SmppClientBase.
 
## [2.6.8] - 2019-03-27
### Fixed
- wrong text splitting in SMS builder for GSMPackedEncoding. 

## [2.6.7] - 2019-03-27
### Fixed
- StackOverflowException by submitting array of SubmitMulti.
- destination addresses serialization for SubmitMulti
- short message length calculation 

## [2.6.6] - 2019-03-25
### Fixed
- exception in GetMessageText method for DeliverSm with empty text.


## [2.6.5] - 2019-03-18
### Fixed
- exception in GetMessageText method for DeliverSm without receipt.


## [2.6.4] - 2019-03-15
### Fixed
- missed last character in the last segment of the concatenated message created with SMS builders.

### Added
- Extension method smppPdu.GetMessageText(EncodingMapper) as replacement for MessageText property in a PDU class.
- TLVCollection.RegisterParameter<TParameter>(ushort tag) method for registering custom TLV parameter type for any tag value.
  It helps to represent some complex parameters as structured objects. Example: var parameter = pdu.Parameters.Of<MyTLVParameter>();

### Changed
- MessageText property in PDU classes is obsolete. Use the function client.EncodingMapper.GetMessageText(pdu) or pdu.GetMessageText(client.EncodingMapper)  to get th message text contained in the PDU.


## [2.6.3] - 2019-03-04
### Fixed
- failed to raise some events with attached delegate that doesn't has target object.

### Improved
- FileLogger multi-threading improvements.

## [2.6.2] - 2019-02-07
### Added
- ILogFactory interface with implementations for File and Console

### Fixed
- client hangs by Dispose when it was never connected
 
## [2.6.1] - 2019-02-04
### Fixed
- Cannot send 160 characters in one part SMS in GSM Encoding 


## [2.6.0] - 2019-01-14
### Added
- ProxyProtocolEnabled property for SmppServerClient class. This property should be enabled in evClientConnected event handler to detect proxy protocol in the network stream of connected client. 
- Signed with Strong Name 
- ClonePDU, Serialize methods for SmppPDU classes.
- SMS.ForData method for building concatenated DataSm PDUs.
- SMS.ForDeliver is able to create delivery receipt in MessagePayload parameter.
 
### Fixed
- SmppServer stops accepting new connections by invalid handshake
- Text splitter for building concatenated message parts
- Event evClientDataSm didn't raise in the SmppServer. 
- Sometimes SmppServerClient doesn't disconnect properly in SmppServer
- concurrency issues in MessageComposer
- library sends response with status ESME_ROK when SmppServer has no attached event handler for a request PDU. It should send unsuccess status f.i. ESME_RINVCMDID.

### API Changes
- Replaced methods AddMessagePayload, AddSARReferenceNumber, AddSARSequenceNumber, AddSARTotalSegments and AddMoreMessagesToSend with corresponding classes in Inetlab.SMPP.Parameters namespace.
- Renamed the property "Optional" to "Parameters" in PDU classes. (backwards-compatible)
- Removed unnecessary TLV constructor with length parameter. Length is always equal to value array length.
- Removed ISmppMessage interface
- Renamed namespace Inetlab.SMPP.Common.Headers to Inetlab.SMPP.Headers
- Rename propery UserDataPdu to UserData for classes SubmitSm, SubmitMulti DeliverSm, ReplaceSm. (backwards-compatible)
- MessageInPayload method tells SMS builder to send complete message text in message_payload parameter. With optional messageSize method parameter you can decrease the size of message segment if you need to send concatenation in SAR parameters.
- Simplified ILog interface



## [2.5.4] - 2018-09-16
### Changed
- MessageComposer.Timeout property to TimeStamp

### Added
- SmppClient.Submit methods with IEnumerable parameter
- better documentation

### Fixed
- Hanlde SocketException OperationAborted when server stops

## [2.5.3] - 2018-09-08
### Fixed
- SubmitSpeedLimit is ignored
- sometimes SMPP PDU reading is failed

## [2.5.2] - 2018-08-06
### Fixed
- Messages with data coding Class0 (0xF0) are split up in wrong way

## [2.5.1] - 2018-07-30
### Fixed
- wrong BindingMode for SmppServerClient after Unbind. 

## [2.5.0] - 2018-07-29
### Added
- Automatic detection for Proxy protocol https://www.haproxy.com/blog/haproxy/proxy-protocol/
### Implemented
- Unbind logic for SmppClient and SmppServerClient classes

## [2.4.1] - 2018-06-19
### Fixed
- issue with licensing module

## [2.4.0] - 2018-05-30
### Added
- Automatic connection recovery.

## [2.3.2] - 2018-04-20
### Added
-  MessageComposer allows to get its items for concatenated messages.
### Changed 
-  creation for user data headers types.

## [2.3.1] - 2018-04-18
### Fixed
- PDU reader and writer
- split text on concatenation parts                   

## [2.3.0] - 2018-03-18
### Added
- SmppClientBase.SendQueueLimit limits the number of sending SMPP messages to remote side. Delays further SMPP requests when limit is exceeded.

### Changed
- SmppServerClient.ReceiveQueueLimit replaced with SmppClientBase.ReceivedRequestQueueLimit

### Improved
- improved: processing of connect and disconnect.

## [2.2.0] - 2018-02-01
### Improved
- better processing of request and response PDU

### Changed
- Flow Control. SmppServerClient.ReceiveQueueLimit defines allowed number of SMPP requests in receive queue. 
       If receive queueu is full, library stops receive from network buffer and waits until queue has a place again.
       It is better alternative for ESME_RMSGQFUL response status.
### Fixed
- MessageComposer raises evFullMessageReceived sometimes two times by processing concatenated message with two parts.

## [2.1.2] - 2017-12-11
### Improved
- internal queue for processing PDU.

## [2.1.1] - 2017-12-10
### Improved
- processing of connect and disconnect 

### Added
- From and To methods with SmeAddress parameter to SMS Builders

## [2.1.0] - 2017-10-18
### Added
- SendSpeedLimit property for SmppClientBase class, that limits number of requests per second to remote side 
- Priority processing for response PDUs.
- Name property to distinguish instances in logger
- Deliver method in SmmpServerClient class
- SubmitData method in SmppClientBase class

## [2.0.1] - 2017-10-06
### Added
- decode receipt for IntermediateDeliveryNotification

### Fixed
- sequence number generation

### [2.0.0] - 2017-08-15
- first version for .NET Standard 1.4