using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.PLater.Uk
{
	[XmlRoot("GetPayLaterSecciAgreement")]
	public partial class GetPayLaterSecciAgreementUkQuery : ApiRequest<GetPayLaterSecciAgreementUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
