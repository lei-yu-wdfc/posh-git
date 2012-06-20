using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Scotiabank.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.AccountRenumberingReportAccountDetailsMessage </summary>
    [XmlRoot("AccountRenumberingReportAccountDetailsMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "")]
    public partial class AccountRenumberingReportDetailsCaCommand : MsmqMessage<AccountRenumberingReportDetailsCaCommand>
    {
        public Object AccountRenumberingReportAccountDetails { get; set; }
        public String Filename { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
