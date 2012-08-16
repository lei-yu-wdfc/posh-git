using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetPasswordRecoveryDetails")]
	public partial class GetPasswordRecoveryDetailsQuery : ApiRequest<GetPasswordRecoveryDetailsQuery>
	{
		public Object AccountId { get; set; }
	}
}
