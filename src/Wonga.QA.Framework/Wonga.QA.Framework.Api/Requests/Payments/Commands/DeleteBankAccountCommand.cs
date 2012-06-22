using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.DeleteBankAccount </summary>
    [XmlRoot("DeleteBankAccount")]
    public partial class DeleteBankAccountCommand : ApiRequest<DeleteBankAccountCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
    }
}
