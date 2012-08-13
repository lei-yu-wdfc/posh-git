using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.IWantToAddPrepaidCardDetailsInternalEvent </summary>
    [XmlRoot("IWantToAddPrepaidCardDetailsInternalEvent", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Salesforce.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToAddPrepaidCardDetailsInternalEvent : MsmqMessage<IWantToAddPrepaidCardDetailsInternalEvent>
    {
        public String CardSerialNumber { get; set; }
        public String CardAccountNumber { get; set; }
        public Guid CardDetailsExternalId { get; set; }
        public Guid CardHolderDetailsExternalId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime ApplicationDate { get; set; }
    }
}
