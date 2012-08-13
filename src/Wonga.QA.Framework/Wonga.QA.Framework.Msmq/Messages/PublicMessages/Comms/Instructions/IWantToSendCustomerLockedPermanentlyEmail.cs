using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendCustomerLockedPermanentlyEmail </summary>
    [XmlRoot("IWantToSendCustomerLockedPermanentlyEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendCustomerLockedPermanentlyEmail : MsmqMessage<IWantToSendCustomerLockedPermanentlyEmail>
    {
        public Guid AccountId { get; set; }
        public Guid MessageFileId { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
