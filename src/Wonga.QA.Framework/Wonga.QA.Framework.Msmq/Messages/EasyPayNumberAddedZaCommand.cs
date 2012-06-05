using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Salesforce.InternalMessages.Za.EasyPayNumberAddedMessage </summary>
    [XmlRoot("EasyPayNumberAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages.Za", DataType = "")]
    public partial class EasyPayNumberAddedZaCommand : MsmqMessage<EasyPayNumberAddedZaCommand>
    {
        public Guid AccountId { get; set; }
    }
}
