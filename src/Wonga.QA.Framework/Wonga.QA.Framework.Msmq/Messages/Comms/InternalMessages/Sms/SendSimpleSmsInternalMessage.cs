using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Sms
{
    /// <summary> Wonga.Comms.InternalMessages.Sms.SendSimpleSmsInternalMessage </summary>
    [XmlRoot("SendSimpleSmsInternalMessage", Namespace = "Wonga.Comms.InternalMessages.Sms", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Sms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendSimpleSmsInternalMessage : MsmqMessage<SendSimpleSmsInternalMessage>
    {
        public SimpleSmsEnum SimpleSmsType { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
    }
}
