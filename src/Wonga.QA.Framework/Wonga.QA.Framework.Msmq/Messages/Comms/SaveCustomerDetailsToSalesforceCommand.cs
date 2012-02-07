using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SaveCustomerDetailsToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class SaveCustomerDetailsToSalesforceCommand : MsmqMessage<SaveCustomerDetailsToSalesforceCommand>
    {
        public Guid AccountId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TitleEnum? Title { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String MiddleName { get; set; }
        public String HomePhone { get; set; }
        public String MobilePhone { get; set; }
        public String WorkPhone { get; set; }
        public String Email { get; set; }
    }
}
