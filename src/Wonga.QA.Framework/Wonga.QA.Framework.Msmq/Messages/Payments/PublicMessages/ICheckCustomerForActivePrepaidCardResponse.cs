using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.ICheckCustomerForActivePrepaidCardResponse </summary>
    [XmlRoot("ICheckCustomerForActivePrepaidCardResponse", Namespace = "Wonga.Payments.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ICheckCustomerForActivePrepaidCardResponse : MsmqMessage<ICheckCustomerForActivePrepaidCardResponse>
    {
        public Guid ApplicationId { get; set; }
        public Boolean CustomerHaveActivePrepaidCard { get; set; }
    }
}
