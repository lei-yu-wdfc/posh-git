using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.ILoanAgreementProduced </summary>
    [XmlRoot("ILoanAgreementProduced", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ILoanAgreementProduced : MsmqMessage<ILoanAgreementProduced>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
