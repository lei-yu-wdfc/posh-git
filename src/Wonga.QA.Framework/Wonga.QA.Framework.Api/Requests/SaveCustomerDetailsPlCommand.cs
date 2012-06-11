using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Pl.SaveCustomerDetails </summary>
    [XmlRoot("SaveCustomerDetails")]
    public partial class SaveCustomerDetailsPlCommand : ApiRequest<SaveCustomerDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object DateOfBirth { get; set; }
        public Object Title { get; set; }
        public Object Gender { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MiddleName { get; set; }
        public Object HomePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object Email { get; set; }
        public Object PeselNumber { get; set; }
        public Object DocumentId { get; set; }
        public Object EducationalLevel { get; set; }
        public Object MobilePhone { get; set; }
        public Object VehicleOwner { get; set; }
        public Object Facebook { get; set; }
        public Object Allegro { get; set; }
        public Object MarketingSource { get; set; }
    }
}
