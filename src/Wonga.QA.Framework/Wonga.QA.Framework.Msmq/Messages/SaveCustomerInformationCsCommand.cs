using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Csapi.Commands.SaveCustomerInformationCsApiMessage </summary>
    [XmlRoot("SaveCustomerInformationCsApiMessage", Namespace = "Wonga.Comms.Csapi.Commands", DataType = "")]
    public partial class SaveCustomerInformationCsCommand : MsmqMessage<SaveCustomerInformationCsCommand>
    {
        public Guid AccountId { get; set; }
        public Guid? AddressId { get; set; }
        public TitleEnum? Title { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Middlename { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String HomePhone { get; set; }
        public String MobilePhone { get; set; }
        public String WorkPhone { get; set; }
        public String Postcode { get; set; }
        public String Town { get; set; }
        public String Street { get; set; }
        public String HouseName { get; set; }
        public String HouseNumber { get; set; }
        public String Flat { get; set; }
        public String NationalNumber { get; set; }
        public String PreferredLanguage { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
