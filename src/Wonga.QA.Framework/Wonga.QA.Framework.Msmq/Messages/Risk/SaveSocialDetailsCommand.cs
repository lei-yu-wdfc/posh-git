using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("SaveSocialDetailsMessage", Namespace = "Wonga.Risk", DataType = "")]
    public class SaveSocialDetailsCommand : MsmqMessage<SaveSocialDetailsCommand>
    {
        public Guid AccountId { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public String VehicleRegistration { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
