using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.SaveServerSideTrackingDevice </summary>
    [XmlRoot("SaveServerSideTrackingDevice")]
    public partial class SaveServerSideTrackingDeviceCommand : ApiRequest<SaveServerSideTrackingDeviceCommand>
    {
        public Object SessionId { get; set; }
        public Object DeviceType { get; set; }
        public Object DeviceGroup { get; set; }
    }
}
