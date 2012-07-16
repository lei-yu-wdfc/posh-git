using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.ApiCore.Commands
{
    /// <summary> Wonga.Risk.ApiCore.Commands.SaveSocialDetailsMessageBase </summary>
    [XmlRoot("SaveSocialDetailsMessageBase", Namespace = "Wonga.Risk.ApiCore.Commands", DataType = "")]
    public partial class SaveSocialDetailsMessageBase : MsmqMessage<SaveSocialDetailsMessageBase>
    {
        public Guid AccountId { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
