using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.CreateFixedTermLoanAgreementMessage </summary>
    [XmlRoot("CreateFixedTermLoanAgreementMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreateFixedTermLoanAgreementCommand : MsmqMessage<CreateFixedTermLoanAgreementCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
