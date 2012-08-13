using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.IHomePhoneUpdatedInternal </summary>
    [XmlRoot("IHomePhoneUpdatedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IHomePhoneUpdated" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IHomePhoneUpdatedInternal : MsmqMessage<IHomePhoneUpdatedInternal>
    {
        public Guid VerificationId { get; set; }
        public String HomePhone { get; set; }
        public Guid AccountId { get; set; }
    }
}
