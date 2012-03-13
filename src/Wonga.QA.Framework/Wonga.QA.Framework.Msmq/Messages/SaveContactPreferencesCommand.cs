using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.SaveContactPreferencesMessage </summary>
    [XmlRoot("SaveContactPreferencesMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class SaveContactPreferencesCommand : MsmqMessage<SaveContactPreferencesCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean AcceptMarketingContact { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
