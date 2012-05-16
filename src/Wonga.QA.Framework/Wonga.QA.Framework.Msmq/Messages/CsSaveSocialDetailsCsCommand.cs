using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Csapi.Commands.CsSaveSocialDetailsMessage </summary>
    [XmlRoot("CsSaveSocialDetailsMessage", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class CsSaveSocialDetailsCsCommand : MsmqMessage<CsSaveSocialDetailsCsCommand>
    {
        public Guid AccountId { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}