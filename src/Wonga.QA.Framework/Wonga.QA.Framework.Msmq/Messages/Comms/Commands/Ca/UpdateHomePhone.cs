using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.UpdateHomePhoneCaMessage </summary>
    [XmlRoot("UpdateHomePhoneCaMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateHomePhone : MsmqMessage<UpdateHomePhone>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
