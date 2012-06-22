using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.AssociateTrackingSessionWithApplicationId </summary>
    [XmlRoot("AssociateTrackingSessionWithApplicationId")]
    public partial class AssociateTrackingSessionWithApplicationIdCommand : ApiRequest<AssociateTrackingSessionWithApplicationIdCommand>
    {
        public Object TrackingSession { get; set; }
        public Object ApplicationId { get; set; }
    }
}
