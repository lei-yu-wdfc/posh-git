using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Queries
{
	[XmlRoot("GetTrackingReference")]
	public partial class GetTrackingReferenceQuery : ApiRequest<GetTrackingReferenceQuery>
	{
		public Object TrackingSession { get; set; }
	}
}
