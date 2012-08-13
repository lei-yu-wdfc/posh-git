using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitNumberOfGuarantorsMessage </summary>
    [XmlRoot("SubmitNumberOfGuarantorsMessage", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SubmitNumberOfGuarantors : MsmqMessage<SubmitNumberOfGuarantors>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Int32 NumberOfGuarantors { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
