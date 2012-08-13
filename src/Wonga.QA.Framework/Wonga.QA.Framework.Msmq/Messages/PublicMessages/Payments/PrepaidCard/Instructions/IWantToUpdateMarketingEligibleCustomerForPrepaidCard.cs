using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToUpdateMarketingEligibleCustomerForPrepaidCard </summary>
    [XmlRoot("IWantToUpdateMarketingEligibleCustomerForPrepaidCard", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToUpdateMarketingEligibleCustomerForPrepaidCard : MsmqMessage<IWantToUpdateMarketingEligibleCustomerForPrepaidCard>
    {
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
    }
}
