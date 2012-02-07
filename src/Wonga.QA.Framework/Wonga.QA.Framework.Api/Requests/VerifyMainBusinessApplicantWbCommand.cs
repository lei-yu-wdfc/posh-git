using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("VerifyMainBusinessApplicant")]
    public partial class VerifyMainBusinessApplicantWbCommand : ApiRequest<VerifyMainBusinessApplicantWbCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
    }
}
