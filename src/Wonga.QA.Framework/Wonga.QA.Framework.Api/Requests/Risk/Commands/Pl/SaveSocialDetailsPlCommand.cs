using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.SaveSocialDetails </summary>
    [XmlRoot("SaveSocialDetails")]
    public partial class SaveSocialDetailsPlCommand : ApiRequest<SaveSocialDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object MaritalStatus { get; set; }
        public Object OccupancyStatus { get; set; }
        public Object Dependants { get; set; }
        public Object VehicleOwner { get; set; }
        public Object MotherMaidenName { get; set; }
    }
}
