using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.UpdateMobilePhoneToSalesforceMessage </summary>
    [XmlRoot("UpdateMobilePhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Salesforce, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateMobilePhoneToSalesforceMessage : MsmqMessage<UpdateMobilePhoneToSalesforceMessage>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public Boolean MobileVerified { get; set; }
        public String Pin { get; set; }
    }
}
