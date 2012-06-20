using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Risk;
using Wonga.QA.Framework.Msmq.Enums.Risk.ApiCore.Commands;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SaveSocialDetailsMessage </summary>
    [XmlRoot("SaveSocialDetailsMessage", Namespace = "Wonga.Risk", DataType = "")]
    public partial class SaveSocialDetailsCommand : MsmqMessage<SaveSocialDetailsCommand>
    {
        public String VehicleRegistration { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public Guid AccountId { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
