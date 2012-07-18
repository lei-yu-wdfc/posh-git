using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands.PLater.Uk
{
    /// <summary> Wonga.Payments.Csapi.Commands.PLater.Uk.ExecuteMerchantRefund </summary>
    [XmlRoot("ExecuteMerchantRefund")]
    public partial class ExecuteMerchantRefundUkCommand : CsRequest<ExecuteMerchantRefundUkCommand>
    {
        public Object ApplicationId { get; set; }
        public Object RefundAmount { get; set; }
    }
}
