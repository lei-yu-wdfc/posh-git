using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitNumberOfGuarantorsMessage </summary>
    [XmlRoot("SubmitNumberOfGuarantorsMessage", Namespace = "Wonga.Risk", DataType = "")]
    public partial class SubmitNumberOfGuarantorsCommand : MsmqMessage<SubmitNumberOfGuarantorsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Int32 NumberOfGuarantors { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
