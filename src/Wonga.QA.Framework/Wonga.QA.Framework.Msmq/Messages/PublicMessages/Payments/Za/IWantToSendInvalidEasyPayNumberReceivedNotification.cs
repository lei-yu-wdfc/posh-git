using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToSendInvalidEasyPayNumberReceivedNotification </summary>
    [XmlRoot("IWantToSendInvalidEasyPayNumberReceivedNotification", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendInvalidEasyPayNumberReceivedNotification : MsmqMessage<IWantToSendInvalidEasyPayNumberReceivedNotification>
    {
        public Int32 AcknowledgeId { get; set; }
    }
}
