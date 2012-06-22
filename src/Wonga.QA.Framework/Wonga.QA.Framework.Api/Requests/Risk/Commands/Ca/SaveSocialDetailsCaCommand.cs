using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca
{
    /// <summary> Wonga.Risk.Commands.Ca.SaveSocialDetails </summary>
    [XmlRoot("SaveSocialDetails")]
    public partial class SaveSocialDetailsCaCommand : ApiRequest<SaveSocialDetailsCaCommand>
    {
        public Object AccountId { get; set; }
        public Object MaritalStatus { get; set; }
        public Object OccupancyStatus { get; set; }
        public Object Dependants { get; set; }
        public Object VehicleRegistration { get; set; }
    }
}
