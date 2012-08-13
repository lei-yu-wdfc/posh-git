using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.ColdStorage
{
    /// <summary> Wonga.PublicMessages.Payments.ColdStorage.ISecureCardDetailsAdded </summary>
    [XmlRoot("ISecureCardDetailsAdded", Namespace = "Wonga.PublicMessages.Payments.ColdStorage", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.ColdStorage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ISecureCardDetailsAdded : MsmqMessage<ISecureCardDetailsAdded>
    {
        public Guid PaymentCardId { get; set; }
    }
}
