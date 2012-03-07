using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Salesforce
{
    /// <summary> Wonga.Salesforce.InternalMessages.EmploymentDetailsSalesforceAccountAddedMessage </summary>
    [XmlRoot("EmploymentDetailsSalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class EmploymentDetailsSalesforceAccountAddedCommand : MsmqMessage<EmploymentDetailsSalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}
