using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.InsertMissingCustomerDetailsMessage </summary>
    [XmlRoot("InsertMissingCustomerDetailsMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class InsertMissingCustomerDetailsZa : MsmqMessage<InsertMissingCustomerDetailsZa>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
