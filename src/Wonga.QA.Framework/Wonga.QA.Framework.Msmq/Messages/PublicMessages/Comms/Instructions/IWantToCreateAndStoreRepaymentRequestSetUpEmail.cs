using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreRepaymentRequestSetUpEmail </summary>
    [XmlRoot("IWantToCreateAndStoreRepaymentRequestSetUpEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreRepaymentRequestSetUpEmail : MsmqMessage<IWantToCreateAndStoreRepaymentRequestSetUpEmail>
    {
        public Guid RepaymentRequestId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
