using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateMarketingEligibleCustomerForPrepaidCard </summary>
    [XmlRoot("IWantToUpdateMarketingEligibleCustomerForPrepaidCard", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToUpdateMarketingEligibleCustomerForPrepaidCardEvent : MsmqMessage<IWantToUpdateMarketingEligibleCustomerForPrepaidCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
