using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
	[XmlRoot("GetEmploymentDetails")]
	public partial class GetEmploymentDetailsQuery : ApiRequest<GetEmploymentDetailsQuery>
	{
		public Object AccountId { get; set; }
	}
}
