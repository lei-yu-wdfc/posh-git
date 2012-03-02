using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreFirstPaymentFailedEmailMessageCompleted", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PaymentRequestFailures", DataType = "")]
    public partial class CreateAndStoreFirstPaymentFailedEmailCompletedWbUkCommand : MsmqMessage<CreateAndStoreFirstPaymentFailedEmailCompletedWbUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid FileId { get; set; }
    }
}
