using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("RecordFinalSuccessMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk.FileWatcherMessages", DataType = "")]
    public partial class RecordFinalSuccessUkCommand : MsmqMessage<RecordFinalSuccessUkCommand>
    {
        public Int32 TransactionId { get; set; }
    }
}
