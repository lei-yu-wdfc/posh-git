using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreRepaymentRequestSetUpEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreRepaymentRequestSetUpEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreRepaymentRequestSetUpEmailResponse : MsmqMessage<IWantToCreateAndStoreRepaymentRequestSetUpEmailResponse>
    {
        public Guid RepaymentRequestId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}
