using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.SaveServerSidePageTrackingData </summary>
    [XmlRoot("SaveServerSidePageTrackingData")]
    public partial class SaveServerSidePageTrackingDataCommand : ApiRequest<SaveServerSidePageTrackingDataCommand>
    {
        public Object SessionId { get; set; }
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object Uri { get; set; }
    }
}
