using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PayLater.CsApi.Commands.Uk
{
    /// <summary> Wonga.Payments.PayLater.CsApi.Commands.Uk.ExecuteMerchantRefund </summary>
    [XmlRoot("ExecuteMerchantRefund", Namespace = "Wonga.Payments.PayLater.CsApi.Commands.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.PayLater.CsApi.Commands.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExecuteMerchantRefund : MsmqMessage<ExecuteMerchantRefund>
    {
        public Guid ApplicationId { get; set; }
        public Decimal RefundAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
