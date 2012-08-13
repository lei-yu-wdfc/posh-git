using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalEvents
{
    /// <summary> Wonga.Salesforce.InternalEvents.ISalesforceAccountAdded </summary>
    [XmlRoot("ISalesforceAccountAdded", Namespace = "Wonga.Salesforce.InternalEvents", DataType = "" )
    , SourceAssembly("Wonga.Salesforce.InternalEvents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ISalesforceAccountAdded : MsmqMessage<ISalesforceAccountAdded>
    {
        public Guid AccountId { get; set; }
    }
}
