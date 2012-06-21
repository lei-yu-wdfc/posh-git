using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.Ca
{
    /// <summary> Wonga.FileStorage.Queries.Ca.GetPreApprovedDirectDebitForm </summary>
    [XmlRoot("GetPreApprovedDirectDebitForm")]
    public partial class GetPreApprovedDirectDebitFormCaQuery : ApiRequest<GetPreApprovedDirectDebitFormCaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
