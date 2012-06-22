using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationDeclined </summary>
    [XmlRoot("ConfirmManualVerificationDeclined", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class ConfirmManualVerificationDeclinedCsCommand : MsmqMessage<ConfirmManualVerificationDeclinedCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
