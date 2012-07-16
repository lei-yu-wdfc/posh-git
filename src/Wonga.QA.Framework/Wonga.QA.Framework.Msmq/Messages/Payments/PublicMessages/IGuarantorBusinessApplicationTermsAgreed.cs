using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IGuarantorBusinessApplicationTermsAgreed </summary>
    [XmlRoot("IGuarantorBusinessApplicationTermsAgreed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IGuarantorBusinessApplicationTermsAgreed : MsmqMessage<IGuarantorBusinessApplicationTermsAgreed>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
