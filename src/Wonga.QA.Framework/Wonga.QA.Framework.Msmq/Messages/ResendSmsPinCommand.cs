using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.ResendSmsPinMessage </summary>
    [XmlRoot("ResendSmsPinMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class ResendSmsPinCommand : MsmqMessage<ResendSmsPinCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
