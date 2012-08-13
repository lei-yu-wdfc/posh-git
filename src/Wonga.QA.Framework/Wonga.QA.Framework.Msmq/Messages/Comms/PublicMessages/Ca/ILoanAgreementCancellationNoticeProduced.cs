using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Ca
{
    /// <summary> Wonga.Comms.PublicMessages.Ca.ILoanAgreementCancellationNoticeProduced </summary>
    [XmlRoot("ILoanAgreementCancellationNoticeProduced", Namespace = "Wonga.Comms.PublicMessages.Ca", DataType = "" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ILoanAgreementCancellationNoticeProduced : MsmqMessage<ILoanAgreementCancellationNoticeProduced>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
