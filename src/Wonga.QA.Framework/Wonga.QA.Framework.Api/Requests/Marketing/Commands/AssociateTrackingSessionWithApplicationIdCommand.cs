using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	[XmlRoot("AssociateTrackingSessionWithApplicationId")]
	public partial class AssociateTrackingSessionWithApplicationIdCommand : ApiRequest<AssociateTrackingSessionWithApplicationIdCommand>
	{
		public Object TrackingSession { get; set; }
		public Object ApplicationId { get; set; }
	}
}
