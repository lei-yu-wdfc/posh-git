using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmNotFraud </summary>
    [XmlRoot("ConfirmNotFraud", Namespace = "Wonga.Risk.Csapi.Commands", DataType = "")]
    public partial class ConfirmNotFraudCsCommand : MsmqMessage<ConfirmNotFraudCsCommand>
    {
        public Guid AccountId { get; set; }
        public String CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
