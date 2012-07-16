using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.SaveCustomerDetailsToSalesforceMessage </summary>
    [XmlRoot("SaveCustomerDetailsToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class SaveCustomerDetailsToSalesforceMessage : MsmqMessage<SaveCustomerDetailsToSalesforceMessage>
    {
        public Guid AccountId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TitleEnum? Title { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String MiddleName { get; set; }
        public GenderEnum Gender { get; set; }
        public String HomePhone { get; set; }
        public String MobilePhone { get; set; }
        public String WorkPhone { get; set; }
        public String Email { get; set; }
        public String NationalNumber { get; set; }
        public String PrefferedLanguage { get; set; }
    }
}
