using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.ITopUpAgreementProduced </summary>
    [XmlRoot("ITopUpAgreementProduced", Namespace = "Wonga.Comms.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ITopUpAgreementProduced : MsmqMessage<ITopUpAgreementProduced>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopUpId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
