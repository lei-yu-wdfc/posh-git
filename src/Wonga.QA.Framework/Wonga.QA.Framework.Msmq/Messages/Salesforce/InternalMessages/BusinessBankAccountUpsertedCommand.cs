using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.BusinessBankAccountUpserted </summary>
    [XmlRoot("BusinessBankAccountUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class BusinessBankAccountUpsertedCommand : MsmqMessage<BusinessBankAccountUpsertedCommand>
    {
        public Guid BankAccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
