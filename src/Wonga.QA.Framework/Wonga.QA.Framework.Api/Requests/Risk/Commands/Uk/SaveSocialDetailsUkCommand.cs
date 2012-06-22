using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
    /// <summary> Wonga.Risk.Commands.Uk.SaveSocialDetails </summary>
    [XmlRoot("SaveSocialDetails")]
    public partial class SaveSocialDetailsUkCommand : ApiRequest<SaveSocialDetailsUkCommand>
    {
        public Object AccountId { get; set; }
        public Object MaritalStatus { get; set; }
        public Object OccupancyStatus { get; set; }
        public Object Dependants { get; set; }
        public Object VehicleRegistration { get; set; }
    }
}
