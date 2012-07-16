using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.ICheckCustomerForActivePrepaidCardResponse </summary>
    [XmlRoot("ICheckCustomerForActivePrepaidCardResponse", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class ICheckCustomerForActivePrepaidCardResponse : MsmqMessage<ICheckCustomerForActivePrepaidCardResponse>
    {
        public Guid ApplicationId { get; set; }
        public Boolean CustomerHaveActivePrepaidCard { get; set; }
    }
}
