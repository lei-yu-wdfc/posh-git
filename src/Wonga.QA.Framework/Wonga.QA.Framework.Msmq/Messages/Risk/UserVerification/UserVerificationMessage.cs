using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.UserVerification
{
    /// <summary> Wonga.Risk.UserVerification.UserVerificationMessage </summary>
    [XmlRoot("UserVerificationMessage", Namespace = "Wonga.Risk.UserVerification", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UserVerificationMessage : MsmqMessage<UserVerificationMessage>
    {
        public Int32 RiskApplicationId { get; set; }
    }
}
