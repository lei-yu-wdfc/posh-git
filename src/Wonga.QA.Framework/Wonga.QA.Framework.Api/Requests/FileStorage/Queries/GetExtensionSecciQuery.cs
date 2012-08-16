using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
	[XmlRoot("GetExtensionSecci")]
	public partial class GetExtensionSecciQuery : ApiRequest<GetExtensionSecciQuery>
	{
		public Object ExtensionId { get; set; }
	}
}
