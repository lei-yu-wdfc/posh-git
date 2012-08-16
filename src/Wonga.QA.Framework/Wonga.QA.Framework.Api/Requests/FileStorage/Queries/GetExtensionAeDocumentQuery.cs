using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
	[XmlRoot("GetExtensionAEDocument")]
	public partial class GetExtensionAEDocumentQuery : ApiRequest<GetExtensionAEDocumentQuery>
	{
		public Object ExtensionId { get; set; }
	}
}
