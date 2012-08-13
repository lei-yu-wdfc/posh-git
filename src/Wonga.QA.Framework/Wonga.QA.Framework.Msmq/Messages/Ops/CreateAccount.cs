using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.CreateAccountMessage </summary>
    [XmlRoot("CreateAccountMessage", Namespace = "Wonga.Ops", DataType = "" )
    , SourceAssembly("Wonga.Ops.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAccount : MsmqMessage<CreateAccount>
    {
        public Guid AccountId { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
