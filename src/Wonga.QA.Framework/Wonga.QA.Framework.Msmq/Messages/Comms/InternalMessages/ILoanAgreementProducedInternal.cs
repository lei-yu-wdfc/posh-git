using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.ILoanAgreementProducedInternal </summary>
    [XmlRoot("ILoanAgreementProducedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.ILoanAgreementProduced" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ILoanAgreementProducedInternal : MsmqMessage<ILoanAgreementProducedInternal>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
