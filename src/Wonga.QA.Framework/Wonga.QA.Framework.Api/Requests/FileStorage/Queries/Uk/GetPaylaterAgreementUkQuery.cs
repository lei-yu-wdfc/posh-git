using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.PLater.Uk
{
	[XmlRoot("GetPaylaterAgreement")]
	public partial class GetPaylaterAgreementUkQuery : ApiRequest<GetPaylaterAgreementUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
