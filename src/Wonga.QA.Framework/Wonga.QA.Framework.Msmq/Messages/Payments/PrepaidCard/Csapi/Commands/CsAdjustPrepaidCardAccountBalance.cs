using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.Csapi.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Csapi.Commands.CsAdjustPrepaidCardAccountBalance </summary>
    [XmlRoot("CsAdjustPrepaidCardAccountBalance", Namespace = "Wonga.Payments.PrepaidCard.Csapi.Commands", DataType = "" )
    , SourceAssembly("Wonga.Payments.PrepaidCard.Csapi.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CsAdjustPrepaidCardAccountBalance : MsmqMessage<CsAdjustPrepaidCardAccountBalance>
    {
        public Decimal Amount { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid PrepaidCardId { get; set; }
        public Guid BalanceAdjustmentlId { get; set; }
        public String Reason { get; set; }
        public String SalesForceUser { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
