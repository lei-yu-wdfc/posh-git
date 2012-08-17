using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.PublicMessages.Payments.PayLater.UK
{
    /// <summary> Wonga.PublicMessages.Payments.PayLater.Uk.ICustomerTransactionFeeAdded </summary>
    [XmlRoot("ICustomerTransactionFeeAdded", Namespace = "Wonga.PublicMessages.Payments.PayLater.Uk", DataType = "")
    , SourceAssembly("Wonga.PublicMessages.Payments.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ICustomerTransactionFeeAdded : MsmqMessage<ICustomerTransactionFeeAdded>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public decimal TransactionFee { get; set; }
    }
}