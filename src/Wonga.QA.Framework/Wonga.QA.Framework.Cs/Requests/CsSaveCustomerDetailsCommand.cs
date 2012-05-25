using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsSaveCustomerDetails </summary>
    [XmlRoot("CsSaveCustomerDetails")]
    public partial class CsSaveCustomerDetailsCommand : CsRequest<CsSaveCustomerDetailsCommand>
    {
        public Object AccountId { get; set; }
        public Object Title { get; set; }
        public Object Gender { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object Middlename { get; set; }
        public Object DateOfBirth { get; set; }
        public Object HomePhone { get; set; }
        public Object MobilePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object NationalNumber { get; set; }
        public Object PreferredLanguage { get; set; }
    }
}
