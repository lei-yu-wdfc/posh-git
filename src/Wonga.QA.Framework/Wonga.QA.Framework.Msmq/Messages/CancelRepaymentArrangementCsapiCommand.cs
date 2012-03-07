using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.CancelRepaymentArrangementCsapi </summary>
    [XmlRoot("CancelRepaymentArrangementCsapi", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CancelRepaymentArrangementCsapiCommand : MsmqMessage<CancelRepaymentArrangementCsapiCommand>
    {
        public Guid RepaymentArrangementId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
