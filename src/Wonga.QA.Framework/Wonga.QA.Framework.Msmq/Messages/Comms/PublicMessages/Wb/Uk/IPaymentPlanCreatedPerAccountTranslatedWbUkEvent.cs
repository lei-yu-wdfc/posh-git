using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IPaymentPlanCreatedPerAccountTranslated </summary>
    [XmlRoot("IPaymentPlanCreatedPerAccountTranslated", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IPaymentPlanCreatedPerAccountTranslatedWbUkEvent : MsmqMessage<IPaymentPlanCreatedPerAccountTranslatedWbUkEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
