using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.RegisterPrePaidCardEmailCommand </summary>
    [XmlRoot("RegisterPrePaidCardEmailCommand", Namespace = "Wonga.Marketing.Commands", DataType = "" )
    , SourceAssembly("Wonga.Marketing.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RegisterPrePaidCardEmailCommand : MsmqMessage<RegisterPrePaidCardEmailCommand>
    {
        public String Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
