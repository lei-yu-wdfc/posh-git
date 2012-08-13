using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateCustomerDetailsToPps </summary>
    [XmlRoot("IWantToUpdateCustomerDetailsToPps", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToUpdateCustomerDetailsToPps : MsmqMessage<IWantToUpdateCustomerDetailsToPps>
    {
        public Guid CustomerExternalId { get; set; }
        public String OriginalMessageId { get; set; }
    }
}
