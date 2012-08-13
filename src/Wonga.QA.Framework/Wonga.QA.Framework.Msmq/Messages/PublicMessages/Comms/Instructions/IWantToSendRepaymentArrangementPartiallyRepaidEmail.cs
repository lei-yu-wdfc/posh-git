using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendRepaymentArrangementPartiallyRepaidEmail </summary>
    [XmlRoot("IWantToSendRepaymentArrangementPartiallyRepaidEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendRepaymentArrangementPartiallyRepaidEmail : MsmqMessage<IWantToSendRepaymentArrangementPartiallyRepaidEmail>
    {
        public Guid SagaId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
        public Boolean IsEarlyPayment { get; set; }
    }
}
