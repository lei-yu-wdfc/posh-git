using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetSecurityQuestionPairs")]
	public partial class GetSecurityQuestionPairsQuery : ApiRequest<GetSecurityQuestionPairsQuery>
	{
		public Object Login { get; set; }
	}
}
