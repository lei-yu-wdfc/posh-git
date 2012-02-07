using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Marketing
{
    [XmlRoot("SaveTrackingDataCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SaveTrackingDataCommand : MsmqMessage<SaveTrackingDataCommand>
    {
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
