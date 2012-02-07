using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CompleteEmailVerificationMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class CompleteEmailVerificationCommand : MsmqMessage<CompleteEmailVerificationCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ChangeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
