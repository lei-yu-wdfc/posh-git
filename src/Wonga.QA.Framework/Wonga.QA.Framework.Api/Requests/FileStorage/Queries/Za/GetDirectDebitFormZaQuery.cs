using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.Za
{
    /// <summary> Wonga.FileStorage.Queries.Za.GetDirectDebitForm </summary>
    [XmlRoot("GetDirectDebitForm")]
    public partial class GetDirectDebitFormZaQuery : ApiRequest<GetDirectDebitFormZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
