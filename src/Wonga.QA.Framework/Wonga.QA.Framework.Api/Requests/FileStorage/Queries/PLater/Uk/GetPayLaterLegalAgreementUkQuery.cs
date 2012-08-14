using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.PLater.Uk
{
    /// <summary> Wonga.FileStorage.Queries.PLater.Uk.GetPayLaterLegalAgreement </summary>
    [XmlRoot("GetPayLaterLegalAgreement")]
    public partial class GetPayLaterLegalAgreementUkQuery : ApiRequest<GetPayLaterLegalAgreementUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
