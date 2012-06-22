using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
    /// <summary> Wonga.Payments.Queries.Uk.GetLoanExtensionPaymentStatus </summary>
    [XmlRoot("GetLoanExtensionPaymentStatus")]
    public partial class GetLoanExtensionPaymentStatusUkQuery : ApiRequest<GetLoanExtensionPaymentStatusUkQuery>
    {
        public Object ExtensionId { get; set; }
    }
}
