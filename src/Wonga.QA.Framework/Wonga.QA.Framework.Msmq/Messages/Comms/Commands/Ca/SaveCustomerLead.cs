using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums.Ca;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.SaveCustomerLeadMessage </summary>
    [XmlRoot("SaveCustomerLeadMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class SaveCustomerLead : MsmqMessage<SaveCustomerLead>
    {
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public String MobilePhone { get; set; }
        public ProvinceEnum Province { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
