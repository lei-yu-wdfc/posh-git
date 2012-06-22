using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries.PLater.Uk
{
    /// <summary> Wonga.Comms.Queries.PLater.Uk.GetPayLaterLegalAgreement </summary>
    [XmlRoot("GetPayLaterLegalAgreement")]
    public partial class GetPayLaterLegalAgreementPLaterUkQuery : ApiRequest<GetPayLaterLegalAgreementPLaterUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
