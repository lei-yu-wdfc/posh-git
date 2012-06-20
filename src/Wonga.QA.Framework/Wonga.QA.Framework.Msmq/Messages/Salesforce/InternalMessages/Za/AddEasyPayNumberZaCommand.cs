using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages.Za
{
    /// <summary> Wonga.Salesforce.InternalMessages.Za.AddEasyPayNumberMessage </summary>
    [XmlRoot("AddEasyPayNumberMessage", Namespace = "Wonga.Salesforce.InternalMessages.Za", DataType = "")]
    public partial class AddEasyPayNumberZaCommand : MsmqMessage<AddEasyPayNumberZaCommand>
    {
        public Guid AccountId { get; set; }
        public String EasyPayNumber { get; set; }
    }
}
