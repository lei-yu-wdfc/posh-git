using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
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
