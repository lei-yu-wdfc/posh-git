using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CancelArrearsRepresentmentsCsapi </summary>
    [XmlRoot("CancelArrearsRepresentmentsCsapi")]
    public partial class CancelArrearsRepresentmentsCommand : CsRequest<CancelArrearsRepresentmentsCommand>
    {
        public Object ApplicationId { get; set; }
        public Object ArrearsRepaymentId { get; set; }
    }
}
