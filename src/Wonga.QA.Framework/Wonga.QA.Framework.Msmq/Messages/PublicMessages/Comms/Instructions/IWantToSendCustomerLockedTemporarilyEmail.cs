using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendCustomerLockedTemporarilyEmail </summary>
    [XmlRoot("IWantToSendCustomerLockedTemporarilyEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendCustomerLockedTemporarilyEmail : MsmqMessage<IWantToSendCustomerLockedTemporarilyEmail>
    {
        public Guid AccountId { get; set; }
        public Guid MessageFileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
