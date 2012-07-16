using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationResult </summary>
    [XmlRoot("ConfirmManualVerificationResult", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class ConfirmManualVerificationResult : MsmqMessage<ConfirmManualVerificationResult>
    {
        public Guid ApplicationId { get; set; }
        public Byte ProbabilityGood { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
