using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PayLater.Commands.Uk
{
    /// <summary> Wonga.Payments.PayLater.Commands.Uk.CreateApplication </summary>
    [XmlRoot("CreateApplication", Namespace = "Wonga.Payments.PayLater.Commands.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.PayLater.Commands.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreatePaylaterApplication : MsmqMessage<CreatePaylaterApplication>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid MerchantId { get; set; }
        public String MerchantReference { get; set; }
        public String MerchantOrderId { get; set; }
        public Decimal TotalAmount { get; set; }
        public String PostCode { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
