using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.RegisterHardshipMessage </summary>
    [XmlRoot("RegisterHardshipMessage", Namespace = "Wonga.Payments.InternalMessages", DataType = "")]
    public partial class RegisterHardshipCommand : MsmqMessage<RegisterHardshipCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean HasHardship { get; set; }
    }
}
