using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("PutCustomerIntoArrearsMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class PutCustomerIntoArrearsCommand : MsmqMessage<PutCustomerIntoArrearsCommand>
    {
        public Int32 ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
