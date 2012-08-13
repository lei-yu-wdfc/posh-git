using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Merchants.CsApi.Commands.PayLater.Uk
{
    /// <summary> Wonga.Payments.Merchants.CsApi.Commands.PayLater.Uk.CsSaveMerchantBankDetails </summary>
    [XmlRoot("CsSaveMerchantBankDetails", Namespace = "Wonga.Payments.Merchants.CsApi.Commands.PayLater.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.Merchants.CsApi.Commands.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CsSaveMerchantBankDetails : MsmqMessage<CsSaveMerchantBankDetails>
    {
        public Guid MerchantId { get; set; }
        public String AccountName { get; set; }
        public String AccountNumber { get; set; }
        public String BankName { get; set; }
        public String SortCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
