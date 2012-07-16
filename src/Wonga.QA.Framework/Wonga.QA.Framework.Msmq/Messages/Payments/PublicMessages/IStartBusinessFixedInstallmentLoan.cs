using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IStartBusinessFixedInstallmentLoan </summary>
    [XmlRoot("IStartBusinessFixedInstallmentLoan", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IStartBusinessFixedInstallmentLoan : MsmqMessage<IStartBusinessFixedInstallmentLoan>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
