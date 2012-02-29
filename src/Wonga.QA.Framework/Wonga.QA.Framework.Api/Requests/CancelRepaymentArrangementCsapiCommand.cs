using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CancelRepaymentArrangementCsapi")]
    public partial class CancelRepaymentArrangementCsapiCommand : ApiRequest<CancelRepaymentArrangementCsapiCommand>
    {
        public Object RepaymentArrangementId { get; set; }
    }
}
