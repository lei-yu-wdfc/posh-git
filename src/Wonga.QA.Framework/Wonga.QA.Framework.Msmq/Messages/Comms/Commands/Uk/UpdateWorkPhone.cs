using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Uk
{
    /// <summary> Wonga.Comms.Commands.Uk.UpdateWorkPhoneUkMessage </summary>
    [XmlRoot("UpdateWorkPhoneUkMessage", Namespace = "Wonga.Comms.Commands.Uk", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateWorkPhone : MsmqMessage<UpdateWorkPhone>
    {
        public Guid AccountId { get; set; }
        public String WorkPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
