using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.Za
{
    /// <summary> Wonga.PublicMessages.Payments.Za.IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotification </summary>
    [XmlRoot("IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotification", Namespace = "Wonga.PublicMessages.Payments.Za", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotification : MsmqMessage<IWantToSendNoOpenApplicationForEasyPayNumberReceivedNotification>
    {
        public Guid AccountId { get; set; }
        public Int32 AcknowledgeId { get; set; }
    }
}
