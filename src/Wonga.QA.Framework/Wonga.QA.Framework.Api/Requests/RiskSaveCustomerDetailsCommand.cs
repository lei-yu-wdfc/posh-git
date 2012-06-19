using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.RiskSaveCustomerDetailsCommand </summary>
    [XmlRoot("RiskSaveCustomerDetails")]
    public partial class RiskSaveCustomerDetailsCommand : ApiRequest<RiskSaveCustomerDetailsCommand>
    {
        public Object AccountId { get; set; }
        public Object Gender { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MiddleName { get; set; }
        public Object HomePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object Email { get; set; }
        public Object MobilePhone { get; set; }
        public Object DateOfBirth { get; set; }
        public Object MaidenName { get; set; }
    }
}
