using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Commands.SaveServerSideTrackingDeviceCommand </summary>
    [XmlRoot("SaveServerSideTrackingDeviceCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SaveServerSideTrackingDeviceCommand : MsmqMessage<SaveServerSideTrackingDeviceCommand>
    {
        public String SessionIdString { get; set; }
        public String DeviceType { get; set; }
        public String DeviceGroup { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
