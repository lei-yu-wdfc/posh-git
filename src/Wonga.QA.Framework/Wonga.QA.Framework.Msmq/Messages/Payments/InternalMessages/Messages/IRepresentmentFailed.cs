using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.IRepresentmentFailed </summary>
    [XmlRoot("IRepresentmentFailed", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRepresentmentFailed : MsmqMessage<IRepresentmentFailed>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Int32 RepresentmentAttempt { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
