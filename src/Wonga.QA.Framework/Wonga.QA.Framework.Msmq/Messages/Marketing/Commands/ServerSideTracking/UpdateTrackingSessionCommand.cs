using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Marketing.Commands.ServerSideTracking
{
    /// <summary> Wonga.Marketing.Commands.ServerSideTracking.UpdateTrackingSessionCommand </summary>
    [XmlRoot("UpdateTrackingSessionCommand", Namespace = "Wonga.Marketing.Commands.ServerSideTracking", DataType = "")]
    public partial class UpdateTrackingSessionCommand : MsmqMessage<UpdateTrackingSessionCommand>
    {
        public String TrackingSession { get; set; }
        public String TrackingReference { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
