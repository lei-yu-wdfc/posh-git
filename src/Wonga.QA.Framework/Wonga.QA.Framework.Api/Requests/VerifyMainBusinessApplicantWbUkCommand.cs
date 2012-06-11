using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Wb.Uk.VerifyMainBusinessApplicant </summary>
    [XmlRoot("VerifyMainBusinessApplicant")]
    public partial class VerifyMainBusinessApplicantWbUkCommand : ApiRequest<VerifyMainBusinessApplicantWbUkCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
    }
}
