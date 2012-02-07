using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("CreateFixedTermLoanTopup", Namespace = "Wonga.Payments", DataType = "")]
    public partial class CreateFixedTermLoanTopupCommand : MsmqMessage<CreateFixedTermLoanTopupCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FixedTermLoanTopupId { get; set; }
        public Decimal TopupAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
