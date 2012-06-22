using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
    /// <summary> Wonga.Risk.Commands.SubmitApplicationBehaviour </summary>
    [XmlRoot("SubmitApplicationBehaviour")]
    public partial class SubmitApplicationBehaviourCommand : ApiRequest<SubmitApplicationBehaviourCommand>
    {
        public Object ApplicationId { get; set; }
        public Object TermSliderPosition { get; set; }
        public Object AmountSliderPosition { get; set; }
    }
}
