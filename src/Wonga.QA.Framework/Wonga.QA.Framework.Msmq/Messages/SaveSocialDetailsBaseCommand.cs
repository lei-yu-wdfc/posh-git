using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.ApiCore.Commands.SaveSocialDetailsMessageBase </summary>
    [XmlRoot("SaveSocialDetailsMessageBase", Namespace = "Wonga.Risk.ApiCore.Commands", DataType = "")]
    public partial class SaveSocialDetailsBaseCommand : MsmqMessage<SaveSocialDetailsBaseCommand>
    {
        public Guid AccountId { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
