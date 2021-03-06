using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Events
{
    /// <summary> Wonga.Payments.InternalMessages.Events.IPaymentAllocatedToFixedTermApplication </summary>
    [XmlRoot("IPaymentAllocatedToFixedTermApplication", Namespace = "Wonga.Payments.InternalMessages.Events", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPaymentAllocatedToFixedTermApplication : MsmqMessage<IPaymentAllocatedToFixedTermApplication>
    {
        public Guid ApplicationId { get; set; }
    }
}
