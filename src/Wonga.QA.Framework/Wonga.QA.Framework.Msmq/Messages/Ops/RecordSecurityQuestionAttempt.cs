using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.RecordSecurityQuestionAttemptCommand </summary>
    [XmlRoot("RecordSecurityQuestionAttemptCommand", Namespace = "Wonga.Ops", DataType = "" )
    , SourceAssembly("Wonga.Ops.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RecordSecurityQuestionAttempt : MsmqMessage<RecordSecurityQuestionAttempt>
    {
        public Guid FirstSecurityQuestionExternalId { get; set; }
        public Guid SecondSecurityQuestionExternalId { get; set; }
        public String FirstSecurityQuestionAnswer { get; set; }
        public String SecondSecurityQuestionAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
