using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateHomePhoneMessage </summary>
    [XmlRoot("UpdateHomePhoneMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public partial class UpdateHomePhoneZa : MsmqMessage<UpdateHomePhoneZa>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
