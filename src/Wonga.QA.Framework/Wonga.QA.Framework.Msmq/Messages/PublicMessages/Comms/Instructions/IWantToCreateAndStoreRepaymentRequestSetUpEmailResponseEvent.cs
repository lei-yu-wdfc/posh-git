using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreRepaymentRequestSetUpEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreRepaymentRequestSetUpEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreRepaymentRequestSetUpEmailResponseEvent : MsmqMessage<IWantToCreateAndStoreRepaymentRequestSetUpEmailResponseEvent>
    {
        public Guid RepaymentRequestId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}
