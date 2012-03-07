using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Za.SaveCustomerDetailsToSalesforceMessage </summary>
    [XmlRoot("SaveCustomerDetailsToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Za", DataType = "")]
    public partial class SaveCustomerDetailsToSalesforceZaCommand : MsmqMessage<SaveCustomerDetailsToSalesforceZaCommand>
    {
        public String NationalNumber { get; set; }
        public LanguageEnum? HomeLanguage { get; set; }
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
