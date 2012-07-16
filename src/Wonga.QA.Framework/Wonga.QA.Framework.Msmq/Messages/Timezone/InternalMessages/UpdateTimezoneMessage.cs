using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Timezone.InternalMessages
{
    /// <summary> Wonga.Timezone.InternalMessages.UpdateTimezoneMessage </summary>
    [XmlRoot("UpdateTimezoneMessage", Namespace = "Wonga.Timezone.InternalMessages", DataType = "")]
    public partial class UpdateTimezoneMessage : MsmqMessage<UpdateTimezoneMessage>
    {
        public Guid AccountId { get; set; }
        public String CountryCode { get; set; }
        public String PostCode { get; set; }
    }
}
