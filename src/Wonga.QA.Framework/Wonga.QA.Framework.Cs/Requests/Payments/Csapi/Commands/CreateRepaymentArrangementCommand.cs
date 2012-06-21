using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CreateRepaymentArrangementCsapi </summary>
    [XmlRoot("CreateRepaymentArrangementCsapi")]
    public partial class CreateRepaymentArrangementCommand : CsRequest<CreateRepaymentArrangementCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
        public Object EffectiveBalance { get; set; }
        public Object RepaymentAmount { get; set; }
        public Object ArrangementDetails { get; set; }
    }
}
