using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms
{
    /// <summary> Wonga.PublicMessages.Comms.IWantToCreateAndStorePaymentNotificationSms </summary>
    [XmlRoot("IWantToCreateAndStorePaymentNotificationSms", Namespace = "Wonga.PublicMessages.Comms", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStorePaymentNotificationSms : MsmqMessage<IWantToCreateAndStorePaymentNotificationSms>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
