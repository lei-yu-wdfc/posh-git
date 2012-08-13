using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages
{
    /// <summary> Wonga.Ops.PublicMessages.IWantToResetPasswordByEmailAndToken </summary>
    [XmlRoot("IWantToResetPasswordByEmailAndToken", Namespace = "Wonga.Ops.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Ops.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToResetPasswordByEmailAndToken : MsmqMessage<IWantToResetPasswordByEmailAndToken>
    {
        public Guid AccountId { get; set; }
    }
}
