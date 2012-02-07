using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SubmitUidAnswers")]
    public partial class SubmitUidAnswersCommand : ApiRequest<SubmitUidAnswersCommand>
    {
        public Object UserActionId { get; set; }
        public Object Answers { get; set; }
    }
}
