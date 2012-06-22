using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Za
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
