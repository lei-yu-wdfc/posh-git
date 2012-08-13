using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.PublicMessages.Za
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementThankYouDoNotRelendEmailSent </summary>
    [XmlRoot("IRepaymentArrangementThankYouDoNotRelendEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent" )
    , SourceAssembly("Wonga.Email.PublicMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRepaymentArrangementThankYouDoNotRelendEmailSent : MsmqMessage<IRepaymentArrangementThankYouDoNotRelendEmailSent>
    {
        public Guid AccountId { get; set; }
    }
}
