using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.IMobilePhoneUpdatedInternal </summary>
    [XmlRoot("IMobilePhoneUpdatedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IMobilePhoneUpdated" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMobilePhoneUpdatedInternal : MsmqMessage<IMobilePhoneUpdatedInternal>
    {
        public Guid VerificationId { get; set; }
        public String MobilePhone { get; set; }
        public Guid AccountId { get; set; }
    }
}
