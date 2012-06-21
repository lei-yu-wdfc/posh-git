using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
    /// <summary> Wonga.Payments.Queries.Uk.GetLoanExtensionStatus </summary>
    [XmlRoot("GetLoanExtensionStatus")]
    public partial class GetLoanExtensionStatusUkQuery : ApiRequest<GetLoanExtensionStatusUkQuery>
    {
        public Object ExtensionId { get; set; }
    }
}
