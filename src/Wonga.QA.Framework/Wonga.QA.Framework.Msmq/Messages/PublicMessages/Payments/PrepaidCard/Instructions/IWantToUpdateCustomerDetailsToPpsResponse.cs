using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateCustomerDetailsToPpsResponse </summary>
    [XmlRoot("IWantToUpdateCustomerDetailsToPpsResponse", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToUpdateCustomerDetailsToPpsResponse : MsmqMessage<IWantToUpdateCustomerDetailsToPpsResponse>
    {
        public Guid CustomerExternalId { get; set; }
        public String OriginalMessageId { get; set; }
        public Boolean Successful { get; set; }
    }
}
