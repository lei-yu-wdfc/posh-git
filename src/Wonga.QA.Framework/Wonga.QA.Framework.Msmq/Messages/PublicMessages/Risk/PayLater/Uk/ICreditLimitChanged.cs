using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Risk.PayLater.Uk
{
    /// <summary> Wonga.PublicMessages.Risk.PayLater.Uk.ICreditLimitChanged </summary>
    [XmlRoot("ICreditLimitChanged", Namespace = "Wonga.PublicMessages.Risk.PayLater.Uk", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.PublicMessages.Risk.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ICreditLimitChanged : MsmqMessage<ICreditLimitChanged>
    {
        public Guid AccountId { get; set; }
        public Decimal PayLaterCreditLimit { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
