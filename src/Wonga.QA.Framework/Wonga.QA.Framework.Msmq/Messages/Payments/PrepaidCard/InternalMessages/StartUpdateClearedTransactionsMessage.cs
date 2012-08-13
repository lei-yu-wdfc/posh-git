using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.StartUpdateClearedTransactionsMessage </summary>
    [XmlRoot("StartUpdateClearedTransactionsMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.PrepaidCard.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StartUpdateClearedTransactionsMessage : MsmqMessage<StartUpdateClearedTransactionsMessage>
    {
        public Guid Id { get; set; }
    }
}
