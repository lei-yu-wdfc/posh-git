using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SignApplication </summary>
    [XmlRoot("SignApplication", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SignApplication : MsmqMessage<SignApplication>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
