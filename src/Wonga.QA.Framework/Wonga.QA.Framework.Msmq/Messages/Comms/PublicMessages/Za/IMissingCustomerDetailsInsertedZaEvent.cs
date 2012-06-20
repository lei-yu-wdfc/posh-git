using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Za
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IMissingCustomerDetailsInserted </summary>
    [XmlRoot("IMissingCustomerDetailsInserted", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IMissingCustomerDetailsInsertedZaEvent : MsmqMessage<IMissingCustomerDetailsInsertedZaEvent>
    {
        public Guid AccountId { get; set; }
    }
}
