using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.SetBankAccountPrimary </summary>
    [XmlRoot("SetBankAccountPrimary")]
    public partial class SetBankAccountPrimaryCommand : ApiRequest<SetBankAccountPrimaryCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
    }
}
