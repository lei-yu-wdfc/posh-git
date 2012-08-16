using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetAccountByToken")]
	public partial class GetAccountByTokenQuery : ApiRequest<GetAccountByTokenQuery>
	{
		public Object Token { get; set; }
	}
}
