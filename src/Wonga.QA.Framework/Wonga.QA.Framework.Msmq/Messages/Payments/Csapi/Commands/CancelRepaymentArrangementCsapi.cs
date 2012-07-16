using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CancelRepaymentArrangementCsapi </summary>
    [XmlRoot("CancelRepaymentArrangementCsapi", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CancelRepaymentArrangementCsapi : MsmqMessage<CancelRepaymentArrangementCsapi>
    {
        public Guid RepaymentArrangementId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
