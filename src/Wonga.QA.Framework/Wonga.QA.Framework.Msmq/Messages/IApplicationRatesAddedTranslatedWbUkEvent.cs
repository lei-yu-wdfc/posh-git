using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IApplicationRatesAddedTranslated </summary>
    [XmlRoot("IApplicationRatesAddedTranslated", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent")]
    public partial class IApplicationRatesAddedTranslatedWbUkEvent : MsmqMessage<IApplicationRatesAddedTranslatedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
