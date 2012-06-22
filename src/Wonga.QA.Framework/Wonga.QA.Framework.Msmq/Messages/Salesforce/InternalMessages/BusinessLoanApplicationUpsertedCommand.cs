using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.BusinessLoanApplicationUpserted </summary>
    [XmlRoot("BusinessLoanApplicationUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class BusinessLoanApplicationUpsertedCommand : MsmqMessage<BusinessLoanApplicationUpsertedCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
