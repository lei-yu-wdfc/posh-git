using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PayLater.InternalMessages
{
    /// <summary> Wonga.Payments.PayLater.InternalMessages.IPayLaterTermsAgreed </summary>
    [XmlRoot("IPayLaterTermsAgreed", Namespace = "Wonga.Payments.PayLater.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.PayLater.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPayLaterTermsAgreed : MsmqMessage<IPayLaterTermsAgreed>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime SignedOn { get; set; }
    }
}
