using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SubmitApplicationBehaviour")]
    public partial class SubmitApplicationBehaviourCommand : ApiRequest<SubmitApplicationBehaviourCommand>
    {
        public Object ApplicationId { get; set; }
        public Object TermSliderPosition { get; set; }
        public Object AmountSliderPosition { get; set; }
    }
}
