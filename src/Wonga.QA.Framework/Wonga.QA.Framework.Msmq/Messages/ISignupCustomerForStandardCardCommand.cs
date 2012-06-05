using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Marketing.Instructions.SignupCustomerForStandardCard </summary>
    [XmlRoot("SignupCustomerForStandardCard", Namespace = "Wonga.Marketing.Instructions", DataType = "Wonga.PublicMessages.PrepaidCard.Instructions.ISignupCustomerForStandardCard")]
    public partial class ISignupCustomerForStandardCardCommand : MsmqMessage<ISignupCustomerForStandardCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
