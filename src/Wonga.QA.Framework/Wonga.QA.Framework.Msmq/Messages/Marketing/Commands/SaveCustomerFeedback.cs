using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.SaveCustomerFeedbackMessage </summary>
    [XmlRoot("SaveCustomerFeedbackMessage", Namespace = "Wonga.Marketing.Commands", DataType = "" )
    , SourceAssembly("Wonga.Marketing.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveCustomerFeedback : MsmqMessage<SaveCustomerFeedback>
    {
        public Guid AccountId { get; set; }
        public String Feedback { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
