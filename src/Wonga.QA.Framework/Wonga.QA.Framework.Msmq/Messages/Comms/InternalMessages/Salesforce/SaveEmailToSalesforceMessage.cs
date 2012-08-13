using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.SaveEmailToSalesforceMessage </summary>
    [XmlRoot("SaveEmailToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Salesforce, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveEmailToSalesforceMessage : MsmqMessage<SaveEmailToSalesforceMessage>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}
