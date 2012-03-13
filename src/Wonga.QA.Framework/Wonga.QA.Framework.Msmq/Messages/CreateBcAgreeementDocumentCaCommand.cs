using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.BritishColombia.CreateBcAgreeementDocumentMessage </summary>
    [XmlRoot("CreateBcAgreeementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public partial class CreateBcAgreeementDocumentCaCommand : MsmqMessage<CreateBcAgreeementDocumentCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
