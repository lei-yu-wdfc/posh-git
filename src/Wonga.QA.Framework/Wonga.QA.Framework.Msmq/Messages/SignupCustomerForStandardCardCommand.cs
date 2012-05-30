using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Commands.SignupCustomerForStandardCardCommand </summary>
    [XmlRoot("SignupCustomerForStandardCardCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SignupCustomerForStandardCardCommand : MsmqMessage<SignupCustomerForStandardCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
