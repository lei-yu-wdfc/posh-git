using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.RepaymentCollectedMessage </summary>
    [XmlRoot("RepaymentCollectedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RepaymentCollectedMessage : MsmqMessage<RepaymentCollectedMessage>
    {
        public Guid RepaymentDetailId { get; set; }
        public Boolean PaidInFull { get; set; }
    }
}
