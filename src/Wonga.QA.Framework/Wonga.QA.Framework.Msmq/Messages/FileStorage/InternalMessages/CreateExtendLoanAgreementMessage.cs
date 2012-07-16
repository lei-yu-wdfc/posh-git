using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateExtendLoanAgreementMessage </summary>
    [XmlRoot("CreateExtendLoanAgreementMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class CreateExtendLoanAgreementMessage : MsmqMessage<CreateExtendLoanAgreementMessage>
    {
        public Guid ExtensionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid SagaId { get; set; }
    }
}
