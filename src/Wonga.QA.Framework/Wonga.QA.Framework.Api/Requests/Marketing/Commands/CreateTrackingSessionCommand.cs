using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.CreateTrackingSession </summary>
    [XmlRoot("CreateTrackingSession")]
    public partial class CreateTrackingSessionCommand : ApiRequest<CreateTrackingSessionCommand>
    {
        public Object TrackingSession { get; set; }
        public Object TrackingReference { get; set; }
        public Object Device { get; set; }
        public Object Group { get; set; }
        public Object Website { get; set; }
        public Object IpAddress { get; set; }
        public Object UserAgent { get; set; }
        public Object Refferer { get; set; }
    }
}
