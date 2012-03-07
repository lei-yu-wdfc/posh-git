using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateHomePhoneMessage </summary>
    [XmlRoot("UpdateHomePhoneMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public partial class UpdateHomePhoneZaCommand : MsmqMessage<UpdateHomePhoneZaCommand>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
