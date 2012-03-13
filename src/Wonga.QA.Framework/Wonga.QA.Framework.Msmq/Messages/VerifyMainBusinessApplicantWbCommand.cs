using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Commands.VerifyMainBusinessApplicantMessage </summary>
    [XmlRoot("VerifyMainBusinessApplicantMessage", Namespace = "Wonga.Risk.Commands", DataType = "")]
    public partial class VerifyMainBusinessApplicantWbCommand : MsmqMessage<VerifyMainBusinessApplicantWbCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
