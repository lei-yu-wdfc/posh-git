using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.RiskSaveCustomerDetails </summary>
    [XmlRoot("RiskSaveCustomerDetails")]
    public partial class RiskSaveCustomerDetailsPlCommand : ApiRequest<RiskSaveCustomerDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object DateOfBirth { get; set; }
        public Object Gender { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MiddleName { get; set; }
        public Object HomePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object Email { get; set; }
        public Object MobilePhone { get; set; }
        public Object MaidenName { get; set; }
        public Object PeselNumber { get; set; }
        public Object IdentificationDocumentId { get; set; }
    }
}
