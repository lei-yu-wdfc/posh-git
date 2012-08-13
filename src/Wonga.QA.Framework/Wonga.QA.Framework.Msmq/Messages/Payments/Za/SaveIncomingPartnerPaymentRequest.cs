using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Za
{
    /// <summary> Wonga.Payments.Za.SaveIncomingPartnerPaymentRequest </summary>
    [XmlRoot("SaveIncomingPartnerPaymentRequest", Namespace = "Wonga.Payments.Za", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveIncomingPartnerPaymentRequest : MsmqMessage<SaveIncomingPartnerPaymentRequest>
    {
        public Guid ApplicationId { get; set; }
        public String PaymentReference { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
