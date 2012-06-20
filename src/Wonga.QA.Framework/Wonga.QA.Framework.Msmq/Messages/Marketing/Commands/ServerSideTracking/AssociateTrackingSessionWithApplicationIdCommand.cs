using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Marketing.Commands.ServerSideTracking
{
    /// <summary> Wonga.Marketing.Commands.ServerSideTracking.AssociateTrackingSessionWithApplicationIdCommand </summary>
    [XmlRoot("AssociateTrackingSessionWithApplicationIdCommand", Namespace = "Wonga.Marketing.Commands.ServerSideTracking", DataType = "")]
    public partial class AssociateTrackingSessionWithApplicationIdCommand : MsmqMessage<AssociateTrackingSessionWithApplicationIdCommand>
    {
        public String TrackingSession { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
