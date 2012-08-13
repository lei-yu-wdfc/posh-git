using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.ISalesforceActivityParentAddedBase </summary>
    [XmlRoot("ISalesforceActivityParentAddedBase", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Salesforce.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ISalesforceActivityParentAddedBase : MsmqMessage<ISalesforceActivityParentAddedBase>
    {
        public Guid AccountId { get; set; }
    }
}
