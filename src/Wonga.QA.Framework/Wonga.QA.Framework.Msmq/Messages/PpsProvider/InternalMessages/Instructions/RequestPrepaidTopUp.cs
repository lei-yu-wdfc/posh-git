using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.Instructions
{
    /// <summary> Wonga.PpsProvider.InternalMessages.Instructions.RequestPrepaidTopUp </summary>
    [XmlRoot("RequestPrepaidTopUp", Namespace = "Wonga.PpsProvider.InternalMessages.Instructions", DataType = "Wonga.PublicMessages.Payments.PrepaidCard.IRequestPrepaidTopUp" )
    , SourceAssembly("Wonga.PpsProvider.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RequestPrepaidTopUp : MsmqMessage<RequestPrepaidTopUp>
    {
        public Guid CustomerExternalId { get; set; }
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
