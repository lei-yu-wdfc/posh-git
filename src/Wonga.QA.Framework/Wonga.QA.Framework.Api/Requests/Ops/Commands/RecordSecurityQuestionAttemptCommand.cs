using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
    /// <summary> Wonga.Ops.Commands.RecordSecurityQuestionAttempt </summary>
    [XmlRoot("RecordSecurityQuestionAttempt")]
    public partial class RecordSecurityQuestionAttemptCommand : ApiRequest<RecordSecurityQuestionAttemptCommand>
    {
        public Object FirstSecurityQuestionExternalId { get; set; }
        public Object SecondSecurityQuestionExternalId { get; set; }
        public Object FirstSecurityQuestionAnswer { get; set; }
        public Object SecondSecurityQuestionAnswer { get; set; }
    }
}
