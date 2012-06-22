using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtendLoanAgreementResponseMessage </summary>
    [XmlRoot("CreateExtendLoanAgreementResponseMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateExtendLoanAgreementResponseCommand : MsmqMessage<CreateExtendLoanAgreementResponseCommand>
    {
        public Guid SagaId { get; set; }
        public String Content { get; set; }
    }
}
