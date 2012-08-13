using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendArrearsBalanceEmail </summary>
    [XmlRoot("IWantToSendArrearsBalanceEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToSendArrearsBalanceEmail : MsmqMessage<IWantToSendArrearsBalanceEmail>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public ArrearsBalanceEmailEnum EmailType { get; set; }
        public Guid OriginatingSagaId { get; set; }
    }
}
