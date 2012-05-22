using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.PLater.Uk.GetPayLaterLegalAgreement </summary>
    [XmlRoot("GetPayLaterLegalAgreement")]
    public partial class GetPayLaterLegalAgreementUkQuery : ApiRequest<GetPayLaterLegalAgreementUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
