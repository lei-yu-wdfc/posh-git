using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.Ca.UpdateHomePhoneCaMessage </summary>
    [XmlRoot("UpdateHomePhoneCaMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class UpdateHomePhoneCaCommand : MsmqMessage<UpdateHomePhoneCaCommand>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
