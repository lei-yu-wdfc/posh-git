using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Wb.Uk
{
    /// <summary> Wonga.Risk.Commands.Wb.Uk.RiskSaveCustomerDetails </summary>
    [XmlRoot("RiskSaveCustomerDetails")]
    public partial class RiskSaveCustomerDetailsWbUkCommand : ApiRequest<RiskSaveCustomerDetailsWbUkCommand>
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
