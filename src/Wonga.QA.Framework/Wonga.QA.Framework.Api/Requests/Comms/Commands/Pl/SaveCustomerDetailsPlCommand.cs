using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Pl
{
    /// <summary> Wonga.Comms.Commands.Pl.SaveCustomerDetails </summary>
    [XmlRoot("SaveCustomerDetails")]
    public partial class SaveCustomerDetailsPlCommand : ApiRequest<SaveCustomerDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MiddleName { get; set; }
        public Object WorkPhone { get; set; }
        public Object Email { get; set; }
        public Object PeselNumber { get; set; }
        public Object MobilePhone { get; set; }
    }
}
