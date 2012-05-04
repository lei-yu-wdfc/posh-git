using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Commands.SaveCustomerInformation </summary>
    [XmlRoot("SaveCustomerInformation")]
    public partial class SaveCustomerInformationCommand : CsRequest<SaveCustomerInformationCommand>
    {
        public Object AccountId { get; set; }
        public Object Title { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object Middlename { get; set; }
        public Object DateOfBirth { get; set; }
        public Object HomePhone { get; set; }
        public Object MobilePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object NationalNumber { get; set; }
        public Object Postcode { get; set; }
        public Object Town { get; set; }
        public Object Street { get; set; }
        public Object HouseName { get; set; }
        public Object HouseNumber { get; set; }
        public Object Flat { get; set; }
        public Object PreferredLanguage { get; set; }
        public Object RequestType { get; set; }
    }
}
