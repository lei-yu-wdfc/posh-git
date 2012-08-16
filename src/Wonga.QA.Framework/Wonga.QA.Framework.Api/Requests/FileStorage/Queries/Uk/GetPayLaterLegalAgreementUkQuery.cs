using System;
using System.Xml.Serialization;

namespace Wonga.FileStorage.Queries.PLater.Uk
{
	[XmlRoot(GetPayLaterLegalAgreement)]
	public class GetPayLaterLegalAgreementUkQuery : ApiRequest<GetPayLaterLegalAgreementUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
