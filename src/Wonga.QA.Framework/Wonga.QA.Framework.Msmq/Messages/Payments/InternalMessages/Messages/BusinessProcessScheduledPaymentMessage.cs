using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.BusinessProcessScheduledPaymentMessage </summary>
    [XmlRoot("BusinessProcessScheduledPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BusinessProcessScheduledPaymentMessage : MsmqMessage<BusinessProcessScheduledPaymentMessage>
    {
        public Guid ApplicationGuid { get; set; }
        public Guid OrganisationId { get; set; }
        public Decimal CollectAmount { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
