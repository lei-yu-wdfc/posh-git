using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Email.PublicMessages.Za
{
    /// <summary> Wonga.Email.PublicMessages.Za.IRepaymentArrangementPartiallyRepaidEmailSent </summary>
    [XmlRoot("IRepaymentArrangementPartiallyRepaidEmailSent", Namespace = "Wonga.Email.PublicMessages.Za", DataType = "Wonga.Email.PublicMessages.Za.IEmailSent" )
    , SourceAssembly("Wonga.Email.PublicMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRepaymentArrangementPartiallyRepaidEmailSent : MsmqMessage<IRepaymentArrangementPartiallyRepaidEmailSent>
    {
        public Guid AccountId { get; set; }
    }
}
