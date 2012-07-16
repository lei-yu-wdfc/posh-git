using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IAccountIsNotFraud </summary>
    [XmlRoot("IAccountIsNotFraud", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IAccountIsNotFraud : MsmqMessage<IAccountIsNotFraud>
    {
        public Guid AccountId { get; set; }
        public String CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
