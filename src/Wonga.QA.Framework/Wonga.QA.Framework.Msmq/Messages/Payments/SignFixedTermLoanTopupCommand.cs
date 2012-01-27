using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("SignFixedTermLoanTopup", Namespace = "Wonga.Payments", DataType = "")]
    public class SignFixedTermLoanTopupCommand : MsmqMessage<SignFixedTermLoanTopupCommand>
    {
        public Guid AccountId { get; set; }
        public Guid FixedTermLoanTopupId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
