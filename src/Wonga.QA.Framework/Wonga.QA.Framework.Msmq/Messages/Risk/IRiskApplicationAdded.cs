using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskApplicationAdded </summary>
    [XmlRoot("IRiskApplicationAdded", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskApplicationAdded : MsmqMessage<IRiskApplicationAdded>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
