using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Wb.Uk.GetUnsignedApplicationsForAccount </summary>
    [XmlRoot("GetUnsignedApplicationsForAccount")]
    public partial class GetUnsignedApplicationsForAccountWbUkQuery : ApiRequest<GetUnsignedApplicationsForAccountWbUkQuery>
    {
        public Object AccountId { get; set; }
    }
}