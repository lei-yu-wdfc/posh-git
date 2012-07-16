using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.GenerateRepaymentNumber </summary>
    [XmlRoot("GenerateRepaymentNumber", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class GenerateRepaymentNumber : MsmqMessage<GenerateRepaymentNumber>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
