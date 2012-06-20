using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtendLoanAgreement </summary>
    [XmlRoot("IWantToCreateAndStoreExtendLoanAgreement", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToCreateAndStoreExtendLoanAgreementEvent : MsmqMessage<IWantToCreateAndStoreExtendLoanAgreementEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid SagaId { get; set; }
    }
}
