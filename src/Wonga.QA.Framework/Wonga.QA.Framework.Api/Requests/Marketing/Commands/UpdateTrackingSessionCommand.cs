using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	[XmlRoot("UpdateTrackingSession")]
	public partial class UpdateTrackingSessionCommand : ApiRequest<UpdateTrackingSessionCommand>
	{
		public Object TrackingSession { get; set; }
		public Object TrackingReference { get; set; }
	}
}
