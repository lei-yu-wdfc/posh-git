using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.RegenerateLoanDocumentsMessage </summary>
    [XmlRoot("RegenerateLoanDocumentsMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class RegenerateLoanDocuments : MsmqMessage<RegenerateLoanDocuments>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid? TopupId { get; set; }
        public Guid? ExtensionId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
