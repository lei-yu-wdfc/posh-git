using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries.PLater.Uk
{
	[XmlRoot("GetPayLaterLegalAgreement")]
	public partial class GetPayLaterLegalAgreementUkQuery : ApiRequest<GetPayLaterLegalAgreementUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
