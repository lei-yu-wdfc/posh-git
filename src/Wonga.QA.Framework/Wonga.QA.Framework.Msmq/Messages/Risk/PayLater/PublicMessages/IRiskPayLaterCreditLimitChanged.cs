using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.PayLater.PublicMessages
{
    /// <summary> Wonga.Risk.PayLater.PublicMessages.IRiskPayLaterCreditLimitChanged </summary>
    [XmlRoot("IRiskPayLaterCreditLimitChanged", Namespace = "Wonga.Risk.PayLater.PublicMessages", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PayLater.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskPayLaterCreditLimitChanged : MsmqMessage<IRiskPayLaterCreditLimitChanged>
    {
        public Guid AccountId { get; set; }
        public Decimal PayLaterCreditLimit { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
