using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateCustomerGenderMessage </summary>
    [XmlRoot("UpdateCustomerGenderMessage", Namespace = "Wonga.Comms.Commands.Za", DataType = "")]
    public partial class UpdateCustomerGenderZaCommand : MsmqMessage<UpdateCustomerGenderZaCommand>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
