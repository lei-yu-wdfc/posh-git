using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.SaveCustomerLeadMessage </summary>
    [XmlRoot("SaveCustomerLeadMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveCustomerLead : MsmqMessage<SaveCustomerLead>
    {
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public String MobilePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
