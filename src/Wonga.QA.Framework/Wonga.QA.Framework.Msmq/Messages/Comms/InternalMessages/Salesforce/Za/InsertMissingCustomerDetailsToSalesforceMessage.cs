using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Za.InsertMissingCustomerDetailsToSalesforceMessage </summary>
    [XmlRoot("InsertMissingCustomerDetailsToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Za", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Salesforce.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class InsertMissingCustomerDetailsToSalesforceMessage : MsmqMessage<InsertMissingCustomerDetailsToSalesforceMessage>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
    }
}
