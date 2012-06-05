using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Commands.SaveServerSideInitialTrackingDataCommand </summary>
    [XmlRoot("SaveServerSideInitialTrackingDataCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SaveServerSideInitialTrackingDataCommand : MsmqMessage<SaveServerSideInitialTrackingDataCommand>
    {
        public String SessionIdString { get; set; }
        public String UriString { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
