using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.Wb
{
    /// <summary> Wonga.Risk.Commands.Wb.VerifyMainBusinessApplicantMessage </summary>
    [XmlRoot("VerifyMainBusinessApplicantMessage", Namespace = "Wonga.Risk.Commands.Wb", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands.Wb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class VerifyMainBusinessApplicant : MsmqMessage<VerifyMainBusinessApplicant>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
