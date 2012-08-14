using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.PLater.Uk
{
    /// <summary> Wonga.FileStorage.Queries.PLater.Uk.GetPayLaterSecciAgreement </summary>
    [XmlRoot("GetPayLaterSecciAgreement")]
    public partial class GetPayLaterSecciAgreementUkQuery : ApiRequest<GetPayLaterSecciAgreementUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
