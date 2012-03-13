using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.UpdateHomePhoneMessage </summary>
    [XmlRoot("UpdateHomePhoneMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class UpdateHomePhoneCommand : MsmqMessage<UpdateHomePhoneCommand>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
