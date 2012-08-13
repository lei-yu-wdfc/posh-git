using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.IPrepaidCardCreated </summary>
    [XmlRoot("IPrepaidCardCreated", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPrepaidCardCreated : MsmqMessage<IPrepaidCardCreated>
    {
        public Guid CardDetailsId { get; set; }
    }
}
