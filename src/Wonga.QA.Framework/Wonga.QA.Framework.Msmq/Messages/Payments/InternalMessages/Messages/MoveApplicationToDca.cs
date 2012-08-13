using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.MoveApplicationToDca </summary>
    [XmlRoot("MoveApplicationToDca", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class MoveApplicationToDca : MsmqMessage<MoveApplicationToDca>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Boolean ChargeBackOccured { get; set; }
    }
}
