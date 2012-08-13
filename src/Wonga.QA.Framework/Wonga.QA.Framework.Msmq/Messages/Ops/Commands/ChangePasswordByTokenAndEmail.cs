using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.Commands
{
    /// <summary> Wonga.Ops.Commands.ChangePasswordByTokenAndEmailMessage </summary>
    [XmlRoot("ChangePasswordByTokenAndEmailMessage", Namespace = "Wonga.Ops.Commands", DataType = "" )
    , SourceAssembly("Wonga.Ops.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ChangePasswordByTokenAndEmail : MsmqMessage<ChangePasswordByTokenAndEmail>
    {
        public String Token { get; set; }
        public String Email { get; set; }
        public String NewPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
