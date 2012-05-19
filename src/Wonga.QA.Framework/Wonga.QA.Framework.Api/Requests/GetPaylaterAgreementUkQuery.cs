using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.PLater.Uk.GetPaylaterAgreement </summary>
    [XmlRoot("GetPaylaterAgreement")]
    public partial class GetPaylaterAgreementUkQuery : ApiRequest<GetPaylaterAgreementUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
