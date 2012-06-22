using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.SaveSocialDetailsMessage </summary>
    [XmlRoot("SaveSocialDetailsMessage", Namespace = "Wonga.Risk.Commands.Pl", DataType = "")]
    public partial class SaveSocialDetailsCommand : MsmqMessage<SaveSocialDetailsCommand>
    {
        public Boolean VehicleOwner { get; set; }
        public String MotherMaidenName { get; set; }
        public String MaritalStatus { get; set; }
        public Guid AccountId { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
