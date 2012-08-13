using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitMPANDetailsMessage </summary>
    [XmlRoot("SubmitMPANDetailsMessage", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.BaseHandleUserDataMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SubmitMPANDetails : MsmqMessage<SubmitMPANDetails>
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
