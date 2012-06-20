using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateCustomerDetailsToPps </summary>
    [XmlRoot("IWantToUpdateCustomerDetailsToPps", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToUpdateCustomerDetailsPpsEvent : MsmqMessage<IWantToUpdateCustomerDetailsPpsEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public String OriginalMessageId { get; set; }
    }
}
