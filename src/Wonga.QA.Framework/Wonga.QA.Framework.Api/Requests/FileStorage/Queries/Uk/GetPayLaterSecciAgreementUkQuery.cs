using System;
using System.Xml.Serialization;

namespace Wonga.FileStorage.Queries.PLater.Uk
{
	[XmlRoot(GetPayLaterSecciAgreement)]
	public class GetPayLaterSecciAgreementUkQuery : ApiRequest<GetPayLaterSecciAgreementUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
