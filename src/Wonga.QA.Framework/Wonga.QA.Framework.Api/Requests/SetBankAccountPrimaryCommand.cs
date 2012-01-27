using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SetBankAccountPrimary")]
    public class SetBankAccountPrimaryCommand : ApiRequest<SetBankAccountPrimaryCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
    }
}
