using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.SignBusinessApplication </summary>
    [XmlRoot("SignBusinessApplication", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "")]
    public partial class SignBusinessApplication : MsmqMessage<SignBusinessApplication>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
