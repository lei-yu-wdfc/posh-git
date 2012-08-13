using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.INotifyBeforeEndLoan </summary>
    [XmlRoot("INotifyBeforeEndLoan", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class INotifyBeforeEndLoan : MsmqMessage<INotifyBeforeEndLoan>
    {
        public Guid AccountId { get; set; }
        public DateTime RemindDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
