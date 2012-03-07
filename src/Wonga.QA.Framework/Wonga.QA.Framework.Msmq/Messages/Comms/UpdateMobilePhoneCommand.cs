using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.Commands.UpdateMobilePhoneMessage </summary>
    [XmlRoot("UpdateMobilePhoneMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class UpdateMobilePhoneCommand : MsmqMessage<UpdateMobilePhoneCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
