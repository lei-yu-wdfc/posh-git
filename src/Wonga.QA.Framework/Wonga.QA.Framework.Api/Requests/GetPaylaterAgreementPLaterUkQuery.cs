using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.PLater.Uk.GetPaylaterAgreement </summary>
    [XmlRoot("GetPaylaterAgreement")]
    public partial class GetPaylaterAgreementPLaterUkQuery : ApiRequest<GetPaylaterAgreementPLaterUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
