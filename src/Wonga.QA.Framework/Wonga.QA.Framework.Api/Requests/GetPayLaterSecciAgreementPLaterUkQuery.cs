using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.PLater.Uk.GetPayLaterSecciAgreement </summary>
    [XmlRoot("GetPayLaterSecciAgreement")]
    public partial class GetPayLaterSecciAgreementPLaterUkQuery : ApiRequest<GetPayLaterSecciAgreementPLaterUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
