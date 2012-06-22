using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.RevokeApplicationFromDca </summary>
    [XmlRoot("RevokeApplicationFromDca", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class RevokeApplicationFromDcaCsCommand : MsmqMessage<RevokeApplicationFromDcaCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
