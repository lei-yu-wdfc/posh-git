using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Ca.Instructions.Alberta
{
    /// <summary> Wonga.PublicMessages.Comms.Ca.Instructions.Alberta.IWantToGenerateALoanAgreementDocument </summary>
    [XmlRoot("IWantToGenerateALoanAgreementDocument", Namespace = "Wonga.PublicMessages.Comms.Ca.Instructions.Alberta", DataType = "Wonga.PublicMessages.Comms.Instructions.IWantToGenerateALegalDocument")]
    public partial class IWantToGenerateALoanAgreementDocumentCaEvent : MsmqMessage<IWantToGenerateALoanAgreementDocumentCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
