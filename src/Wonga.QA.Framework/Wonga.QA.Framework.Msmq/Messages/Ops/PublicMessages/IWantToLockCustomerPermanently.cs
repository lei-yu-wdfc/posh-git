using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages
{
    /// <summary> Wonga.Ops.PublicMessages.IWantToLockCustomerPermanently </summary>
    [XmlRoot("IWantToLockCustomerPermanently", Namespace = "Wonga.Ops.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Ops.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToLockCustomerPermanently : MsmqMessage<IWantToLockCustomerPermanently>
    {
        public Guid AccountId { get; set; }
    }
}
