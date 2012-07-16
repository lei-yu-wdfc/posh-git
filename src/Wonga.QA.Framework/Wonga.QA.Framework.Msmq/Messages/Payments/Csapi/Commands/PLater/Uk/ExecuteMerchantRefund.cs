using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands.PLater.Uk
{
    /// <summary> Wonga.Payments.Csapi.Commands.PLater.Uk.ExecuteMerchantRefund </summary>
    [XmlRoot("ExecuteMerchantRefund", Namespace = "Wonga.Payments.Csapi.Commands.PLater.Uk", DataType = "")]
    public partial class ExecuteMerchantRefund : MsmqMessage<ExecuteMerchantRefund>
    {
        public Guid ApplicationId { get; set; }
        public Decimal RefundAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
