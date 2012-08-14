using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.PLater.Uk
{
    /// <summary> Wonga.FileStorage.Queries.PLater.Uk.GetPaylaterAgreement </summary>
    [XmlRoot("GetPaylaterAgreement")]
    public partial class GetAgreementUkQuery : ApiRequest<GetAgreementUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
