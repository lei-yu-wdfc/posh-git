using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendRepaymentRequestSetUpEmailResponse </summary>
    [XmlRoot("IWantToSendRepaymentRequestSetUpEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendRepaymentRequestSetUpEmailResponse : MsmqMessage<IWantToSendRepaymentRequestSetUpEmailResponse>
    {
        public Guid RepaymentRequestId { get; set; }
        public Boolean Successful { get; set; }
    }
}
