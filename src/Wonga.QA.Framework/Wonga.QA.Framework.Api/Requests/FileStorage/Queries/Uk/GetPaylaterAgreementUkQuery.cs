using System;
using System.Xml.Serialization;

namespace Wonga.FileStorage.Queries.PLater.Uk
{
	[XmlRoot(GetPaylaterAgreement)]
	public class GetPaylaterAgreementUkQuery : ApiRequest<GetPaylaterAgreementUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
