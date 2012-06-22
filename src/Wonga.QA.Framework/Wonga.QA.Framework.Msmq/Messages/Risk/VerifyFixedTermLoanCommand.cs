using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.VerifyFixedTermLoanMessage </summary>
    [XmlRoot("VerifyFixedTermLoanMessage", Namespace = "Wonga.Risk", DataType = "")]
    public partial class VerifyFixedTermLoanCommand : MsmqMessage<VerifyFixedTermLoanCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
