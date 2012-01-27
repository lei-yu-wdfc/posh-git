using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveSocialDetails")]
    public class SaveSocialDetailsCommand : ApiRequest<SaveSocialDetailsCommand>
    {
        public Object AccountId { get; set; }
        public Object MaritalStatus { get; set; }
        public Object OccupancyStatus { get; set; }
        public Object Dependants { get; set; }
        public Object VehicleRegistration { get; set; }
    }
}
