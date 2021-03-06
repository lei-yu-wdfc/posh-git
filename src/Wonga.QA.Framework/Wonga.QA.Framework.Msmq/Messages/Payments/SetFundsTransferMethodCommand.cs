using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SetFundsTransferMethodCommand </summary>
    [XmlRoot("SetFundsTransferMethodCommand", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SetFundsTransferMethodCommand : MsmqMessage<SetFundsTransferMethodCommand>
    {
        public Guid ApplicationId { get; set; }
        public FundsTransferMethodEnum TransferMethod { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
