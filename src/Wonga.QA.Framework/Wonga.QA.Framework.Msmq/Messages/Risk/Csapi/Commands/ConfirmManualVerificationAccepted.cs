using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationAccepted </summary>
    [XmlRoot("ConfirmManualVerificationAccepted", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class ConfirmManualVerificationAccepted : MsmqMessage<ConfirmManualVerificationAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Byte ProbabilityGood { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
