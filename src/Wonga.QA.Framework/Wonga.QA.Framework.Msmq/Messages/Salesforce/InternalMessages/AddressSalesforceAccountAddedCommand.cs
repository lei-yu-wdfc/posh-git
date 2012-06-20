using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.AddressSalesforceAccountAddedMessage </summary>
    [XmlRoot("AddressSalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class AddressSalesforceAccountAddedCommand : MsmqMessage<AddressSalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}
