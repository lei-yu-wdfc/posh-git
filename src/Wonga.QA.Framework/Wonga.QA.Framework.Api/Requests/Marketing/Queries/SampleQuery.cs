using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Queries
{
	[XmlRoot("Sample")]
	public partial class SampleQuery : ApiRequest<SampleQuery>
	{
		public Object Hello { get; set; }
	}
}
