using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.CreateFixedTermLoanExtension </summary>
    [XmlRoot("CreateFixedTermLoanExtension", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateFixedTermLoanExtension : MsmqMessage<CreateFixedTermLoanExtension>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime ExtendDate { get; set; }
        public Guid PaymentCardId { get; set; }
        public String PaymentCardCv2 { get; set; }
        public Decimal PartPaymentAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
