using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.Za
{
    /// <summary> Wonga.Payments.Za.GenerateEasyPayNumber </summary>
    [XmlRoot("GenerateEasyPayNumber", Namespace = "Wonga.Payments.Za", DataType = "")]
    public partial class GenerateEasyPayNumberZaCommand : MsmqMessage<GenerateEasyPayNumberZaCommand>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
