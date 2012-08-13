using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CardPayment.InternalMessages
{
    /// <summary> Wonga.CardPayment.InternalMessages.SetServiceLoginPrimaryMessage </summary>
    [XmlRoot("SetServiceLoginPrimaryMessage", Namespace = "Wonga.CardPayment.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.CardPayment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SetServiceLoginPrimaryMessage : MsmqMessage<SetServiceLoginPrimaryMessage>
    {
        public Guid ServiceLoginId { get; set; }
    }
}
