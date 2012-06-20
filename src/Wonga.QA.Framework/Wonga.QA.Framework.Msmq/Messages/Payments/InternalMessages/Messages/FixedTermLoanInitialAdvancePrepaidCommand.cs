using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.FixedTermLoanInitialAdvancePrepaidMessage </summary>
    [XmlRoot("FixedTermLoanInitialAdvancePrepaidMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class FixedTermLoanInitialAdvancePrepaidCommand : MsmqMessage<FixedTermLoanInitialAdvancePrepaidCommand>
    {
        public Int32 ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
