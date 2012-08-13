using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages
{
    /// <summary> Wonga.Ops.PublicMessages.IWantToNotifyCustomerLockedPermanently </summary>
    [XmlRoot("IWantToNotifyCustomerLockedPermanently", Namespace = "Wonga.Ops.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Ops.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToNotifyCustomerLockedPermanently : MsmqMessage<IWantToNotifyCustomerLockedPermanently>
    {
        public Guid AccountId { get; set; }
    }
}
