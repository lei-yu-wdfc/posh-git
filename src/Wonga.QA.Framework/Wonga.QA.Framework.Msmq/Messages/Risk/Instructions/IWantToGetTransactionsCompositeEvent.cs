using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Instructions
{
    /// <summary> Wonga.Risk.Instructions.IWantToGetTransactionsComposite </summary>
    [XmlRoot("IWantToGetTransactionsComposite", Namespace = "Wonga.Risk.Instructions", DataType = "")]
    public partial class IWantToGetTransactionsCompositeEvent : MsmqMessage<IWantToGetTransactionsCompositeEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
