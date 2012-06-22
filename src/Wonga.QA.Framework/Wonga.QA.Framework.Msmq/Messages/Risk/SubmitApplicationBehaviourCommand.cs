using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitApplicationBehaviourMessage </summary>
    [XmlRoot("SubmitApplicationBehaviourMessage", Namespace = "Wonga.Risk", DataType = "")]
    public partial class SubmitApplicationBehaviourCommand : MsmqMessage<SubmitApplicationBehaviourCommand>
    {
        public Guid ApplicationId { get; set; }
        public String TermSliderPosition { get; set; }
        public String AmountSliderPosition { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
