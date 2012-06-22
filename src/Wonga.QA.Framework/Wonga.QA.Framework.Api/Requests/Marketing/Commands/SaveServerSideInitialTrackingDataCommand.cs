using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	/// <summary> Wonga.Marketing.Commands.SaveServerSideInitialTrackingData </summary>
	[XmlRoot("SaveServerSideInitialTrackingData")]
	public partial class SaveServerSideInitialTrackingDataCommand : ApiRequest<SaveServerSideInitialTrackingDataCommand>
	{
		public Object SessionId { get; set; }
		public Object Uri { get; set; }
	}
}
