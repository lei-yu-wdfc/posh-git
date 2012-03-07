using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.Alberta.CreateAlbertaAgreeementDocumentMessage </summary>
    [XmlRoot("CreateAlbertaAgreeementDocumentMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.Alberta", DataType = "")]
    public partial class CreateAlbertaAgreeementDocumentCaCommand : MsmqMessage<CreateAlbertaAgreeementDocumentCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
