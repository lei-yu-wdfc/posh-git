using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IBusinessPaymentCardAddedToApplication", Namespace = "Wonga.Risk.Business", DataType = "")]
    public partial class IBusinessPaymentCardAddedToApplicationEvent : MsmqMessage<IBusinessPaymentCardAddedToApplicationEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid BusinessPaymentCardId { get; set; }
    }
}
