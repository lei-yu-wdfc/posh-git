using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.MoveApplicationToDca </summary>
    [XmlRoot("MoveApplicationToDca", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class MoveApplicationToDcaCommand : MsmqMessage<MoveApplicationToDcaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Boolean ChargeBackOccured { get; set; }
    }
}
