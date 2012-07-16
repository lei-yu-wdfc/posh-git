using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateCustomerWorkPhoneMessage </summary>
    [XmlRoot("UpdateCustomerWorkPhoneMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public partial class UpdateCustomerWorkPhoneZa : MsmqMessage<UpdateCustomerWorkPhoneZa>
    {
        public Guid AccountId { get; set; }
        public String WorkPhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
