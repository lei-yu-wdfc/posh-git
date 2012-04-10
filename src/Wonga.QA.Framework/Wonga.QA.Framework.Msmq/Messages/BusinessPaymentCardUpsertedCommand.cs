using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Salesforce.InternalMessages.BusinessPaymentCardUpserted </summary>
    [XmlRoot("BusinessPaymentCardUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class BusinessPaymentCardUpsertedCommand : MsmqMessage<BusinessPaymentCardUpsertedCommand>
    {
        public Guid PaymentCardId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
