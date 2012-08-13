using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Merchants.CsApi.Commands.PayLater.Uk
{
    /// <summary> Wonga.Payments.Merchants.CsApi.Commands.PayLater.Uk.CsSaveMerchantDetails </summary>
    [XmlRoot("CsSaveMerchantDetails", Namespace = "Wonga.Payments.Merchants.CsApi.Commands.PayLater.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.Merchants.CsApi.Commands.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CsSaveMerchantDetails : MsmqMessage<CsSaveMerchantDetails>
    {
        public Guid MerchantId { get; set; }
        public Decimal FeeRate { get; set; }
        public Decimal FundedFeeRate { get; set; }
        public Int32 PaymentTerms { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
