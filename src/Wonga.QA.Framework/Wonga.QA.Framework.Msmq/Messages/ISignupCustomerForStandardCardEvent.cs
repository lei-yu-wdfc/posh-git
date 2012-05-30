using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.PrepaidCard.Instructions.ISignupCustomerForStandardCard </summary>
    [XmlRoot("ISignupCustomerForStandardCard", Namespace = "Wonga.PublicMessages.PrepaidCard.Instructions", DataType = "")]
    public partial class ISignupCustomerForStandardCardEvent : MsmqMessage<ISignupCustomerForStandardCardEvent>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
