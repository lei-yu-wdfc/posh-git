using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtendLoanAgreementResponseMessage </summary>
    [XmlRoot("CreateExtendLoanAgreementResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.FileStorage.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateExtendLoanAgreementResponseMessage : MsmqMessage<CreateExtendLoanAgreementResponseMessage>
    {
        public Guid SagaId { get; set; }
        public String Content { get; set; }
    }
}
