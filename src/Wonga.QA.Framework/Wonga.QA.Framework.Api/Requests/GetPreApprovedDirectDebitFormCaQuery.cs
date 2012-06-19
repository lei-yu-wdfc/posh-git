using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.FileStorage.Queries.Ca.GetPreApprovedDirectDebitForm </summary>
    [XmlRoot("GetPreApprovedDirectDebitForm")]
    public partial class GetPreApprovedDirectDebitFormCaQuery : ApiRequest<GetPreApprovedDirectDebitFormCaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
