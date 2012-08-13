using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.SaveCustomerFeedbackStoryMessage </summary>
    [XmlRoot("SaveCustomerFeedbackStoryMessage", Namespace = "Wonga.Marketing.Commands", DataType = "" )
    , SourceAssembly("Wonga.Marketing.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveCustomerFeedbackStory : MsmqMessage<SaveCustomerFeedbackStory>
    {
        public Guid AccountId { get; set; }
        public String Story { get; set; }
        public String CustomerName { get; set; }
        public Boolean AllowContact { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
