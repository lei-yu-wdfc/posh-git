using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.PayLater.PublicMessages
{
    /// <summary> Wonga.Risk.PayLater.PublicMessages.IRiskPayLaterApplicationAdded </summary>
    [XmlRoot("IRiskPayLaterApplicationAdded", Namespace = "Wonga.Risk.PayLater.PublicMessages", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PayLater.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskPayLaterApplicationAdded : MsmqMessage<IRiskPayLaterApplicationAdded>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
