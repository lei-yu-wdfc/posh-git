using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Ping
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StartDebtSoldPaymentSuppression </summary>
    [XmlRoot("StartDebtSoldPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class StartDebtSoldPaymentSuppression : MsmqMessage<StartDebtSoldPaymentSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
