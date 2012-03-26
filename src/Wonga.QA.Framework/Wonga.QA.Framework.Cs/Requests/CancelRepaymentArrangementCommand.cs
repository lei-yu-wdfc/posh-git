using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CancelRepaymentArrangementCsapi </summary>
    [XmlRoot("CancelRepaymentArrangementCsapi")]
    public partial class CancelRepaymentArrangementCommand : CsRequest<CancelRepaymentArrangementCommand>
    {
        public Object RepaymentArrangementId { get; set; }
    }
}
