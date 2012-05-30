using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.AddGuarantorToApplication </summary>
    [XmlRoot("AddGuarantorToApplication", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "")]
    public partial class AddGuarantorToApplicationWbUkCommand : MsmqMessage<AddGuarantorToApplicationWbUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid GuarantorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
