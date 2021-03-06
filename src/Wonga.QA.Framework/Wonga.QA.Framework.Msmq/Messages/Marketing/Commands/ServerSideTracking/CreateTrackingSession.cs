using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Marketing.Commands.ServerSideTracking
{
    /// <summary> Wonga.Marketing.Commands.ServerSideTracking.CreateTrackingSessionCommand </summary>
    [XmlRoot("CreateTrackingSessionCommand", Namespace = "Wonga.Marketing.Commands.ServerSideTracking", DataType = "" )
    , SourceAssembly("Wonga.Marketing.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateTrackingSession : MsmqMessage<CreateTrackingSession>
    {
        public String TrackingSession { get; set; }
        public String TrackingReference { get; set; }
        public String Device { get; set; }
        public String Group { get; set; }
        public String Website { get; set; }
        public String IpAddress { get; set; }
        public String UserAgent { get; set; }
        public String Refferer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
