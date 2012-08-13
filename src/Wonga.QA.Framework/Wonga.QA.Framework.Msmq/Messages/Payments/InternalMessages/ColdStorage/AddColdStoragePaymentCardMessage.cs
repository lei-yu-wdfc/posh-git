using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.ColdStorage
{
    /// <summary> Wonga.Payments.InternalMessages.ColdStorage.AddColdStoragePaymentCardMessage </summary>
    [XmlRoot("AddColdStoragePaymentCardMessage", Namespace = "Wonga.Payments.InternalMessages.ColdStorage", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages.ColdStorage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddColdStoragePaymentCardMessage : MsmqMessage<AddColdStoragePaymentCardMessage>
    {
        public Guid ExternalId { get; set; }
        public Object CardNumber { get; set; }
        public Object CV2 { get; set; }
    }
}
