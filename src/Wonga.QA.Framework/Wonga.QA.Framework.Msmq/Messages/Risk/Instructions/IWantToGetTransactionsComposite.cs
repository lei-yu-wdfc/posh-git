using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Instructions
{
    /// <summary> Wonga.Risk.Instructions.IWantToGetTransactionsComposite </summary>
    [XmlRoot("IWantToGetTransactionsComposite", Namespace = "Wonga.Risk.Instructions", DataType = "" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToGetTransactionsComposite : MsmqMessage<IWantToGetTransactionsComposite>
    {
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
    }
}
