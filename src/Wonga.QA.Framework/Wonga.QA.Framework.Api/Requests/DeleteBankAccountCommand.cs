using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("DeleteBankAccount")]
    public class DeleteBankAccountCommand : ApiRequest<DeleteBankAccountCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
    }
}
