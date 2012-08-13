using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToCreateAndStoreExtendLoanAgreementResponse </summary>
    [XmlRoot("IWantToCreateAndStoreExtendLoanAgreementResponse", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Comms, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateAndStoreExtendLoanAgreementResponse : MsmqMessage<IWantToCreateAndStoreExtendLoanAgreementResponse>
    {
        public Guid FileId { get; set; }
        public Guid SagaId { get; set; }
    }
}
