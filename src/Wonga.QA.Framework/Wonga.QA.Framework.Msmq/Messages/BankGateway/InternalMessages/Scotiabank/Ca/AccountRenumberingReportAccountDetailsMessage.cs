using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Scotiabank.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.AccountRenumberingReportAccountDetailsMessage </summary>
    [XmlRoot("AccountRenumberingReportAccountDetailsMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Scotiabank.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AccountRenumberingReportAccountDetailsMessage : MsmqMessage<AccountRenumberingReportAccountDetailsMessage>
    {
        public Object AccountRenumberingReportAccountDetails { get; set; }
        public String Filename { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
