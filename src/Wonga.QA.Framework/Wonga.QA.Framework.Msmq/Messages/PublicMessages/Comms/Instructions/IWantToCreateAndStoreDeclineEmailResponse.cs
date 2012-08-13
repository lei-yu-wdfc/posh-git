using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreDeclineEmailResponse </summary>
    [XmlRoot("IWantToCreateAndStoreDeclineEmailResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreDeclineEmailResponse : MsmqMessage<IWantToCreateAndStoreDeclineEmailResponse>
    {
        public Guid SagaId { get; set; }
        public Guid FileId { get; set; }
    }
}
