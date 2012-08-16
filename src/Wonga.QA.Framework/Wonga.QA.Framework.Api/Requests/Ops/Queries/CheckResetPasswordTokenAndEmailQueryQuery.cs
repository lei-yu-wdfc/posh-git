using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("CheckResetPasswordTokenAndEmailQuery")]
	public partial class CheckResetPasswordTokenAndEmailQueryQuery : ApiRequest<CheckResetPasswordTokenAndEmailQueryQuery>
	{
		public Object Token { get; set; }
		public Object Email { get; set; }
	}
}
