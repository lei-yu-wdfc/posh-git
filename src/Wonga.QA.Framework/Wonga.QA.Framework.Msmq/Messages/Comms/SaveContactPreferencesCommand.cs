using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SaveContactPreferencesMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public class SaveContactPreferencesCommand : MsmqMessage<SaveContactPreferencesCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean AcceptMarketingContact { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
