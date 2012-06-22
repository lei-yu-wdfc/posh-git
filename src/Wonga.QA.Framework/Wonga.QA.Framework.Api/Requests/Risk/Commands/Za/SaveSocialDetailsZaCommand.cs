using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Za
{
    /// <summary> Wonga.Risk.Commands.Za.SaveSocialDetails </summary>
    [XmlRoot("SaveSocialDetails")]
    public partial class SaveSocialDetailsZaCommand : ApiRequest<SaveSocialDetailsZaCommand>
    {
        public Object AccountId { get; set; }
        public Object MaritalStatus { get; set; }
        public Object OccupancyStatus { get; set; }
        public Object Dependants { get; set; }
        public Object VehicleRegistration { get; set; }
    }
}
