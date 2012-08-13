using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Za
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IMissingCustomerDetailsInserted </summary>
    [XmlRoot("IMissingCustomerDetailsInserted", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMissingCustomerDetailsInserted : MsmqMessage<IMissingCustomerDetailsInserted>
    {
        public Guid AccountId { get; set; }
    }
}
