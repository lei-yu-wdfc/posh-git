using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.SubmitMPANDetailsMessage </summary>
    [XmlRoot("SubmitMPANDetailsMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseHandleUserDataMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SubmitMpanDetailsCommand : MsmqMessage<SubmitMpanDetailsCommand>
    {
        public String Number1Field { get; set; }
        public String Number2Field { get; set; }
        public String Number3Field { get; set; }
        public String Number4Field { get; set; }
        public String MailSortCode { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
