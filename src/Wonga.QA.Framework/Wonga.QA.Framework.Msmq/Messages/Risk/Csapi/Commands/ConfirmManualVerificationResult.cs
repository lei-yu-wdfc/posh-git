using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationResult </summary>
    [XmlRoot("ConfirmManualVerificationResult", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "" )
    , SourceAssembly("Wonga.Risk.Csapi.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ConfirmManualVerificationResult : MsmqMessage<ConfirmManualVerificationResult>
    {
        public Guid ApplicationId { get; set; }
        public Byte ProbabilityGood { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
