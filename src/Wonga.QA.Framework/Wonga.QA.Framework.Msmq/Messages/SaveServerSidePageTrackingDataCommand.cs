using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Commands.SaveServerSidePageTrackingDataCommand </summary>
    [XmlRoot("SaveServerSidePageTrackingDataCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SaveServerSidePageTrackingDataCommand : MsmqMessage<SaveServerSidePageTrackingDataCommand>
    {
        public String SessionIdString { get; set; }
        public String Uri { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
