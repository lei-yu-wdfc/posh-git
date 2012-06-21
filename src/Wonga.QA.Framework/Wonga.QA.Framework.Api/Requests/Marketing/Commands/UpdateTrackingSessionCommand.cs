using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.UpdateTrackingSession </summary>
    [XmlRoot("UpdateTrackingSession")]
    public partial class UpdateTrackingSessionCommand : ApiRequest<UpdateTrackingSessionCommand>
    {
        public Object TrackingSession { get; set; }
        public Object TrackingReference { get; set; }
    }
}
