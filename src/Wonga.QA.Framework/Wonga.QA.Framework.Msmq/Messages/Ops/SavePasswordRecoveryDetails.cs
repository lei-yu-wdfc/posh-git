using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.SavePasswordRecoveryDetailsBasicMessage </summary>
    [XmlRoot("SavePasswordRecoveryDetailsBasicMessage", Namespace = "Wonga.Ops", DataType = "" )
    , SourceAssembly("Wonga.Ops.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SavePasswordRecoveryDetails : MsmqMessage<SavePasswordRecoveryDetails>
    {
        public Guid AccountId { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
