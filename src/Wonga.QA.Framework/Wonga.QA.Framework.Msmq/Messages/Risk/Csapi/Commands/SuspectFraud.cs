using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.SuspectFraud </summary>
    [XmlRoot("SuspectFraud", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class SuspectFraud : MsmqMessage<SuspectFraud>
    {
        public Guid AccountId { get; set; }
        public String CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
