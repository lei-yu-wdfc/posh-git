using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.CreateFixedTermLoanExtension </summary>
    [XmlRoot("CreateFixedTermLoanExtension", Namespace = "Wonga.Payments", DataType = "")]
    public partial class CreateFixedTermLoanExtensionCommand : MsmqMessage<CreateFixedTermLoanExtensionCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public DateTime ExtendDate { get; set; }
        public Guid PaymentCardId { get; set; }
        public String PaymentCardCv2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
