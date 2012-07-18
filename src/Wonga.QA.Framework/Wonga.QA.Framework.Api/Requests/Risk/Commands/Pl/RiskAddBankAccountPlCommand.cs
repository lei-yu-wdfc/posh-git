using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.RiskAddBankAccount </summary>
    [XmlRoot("RiskAddBankAccount")]
    public partial class RiskAddBankAccountPlCommand : ApiRequest<RiskAddBankAccountPlCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object AccountNumber { get; set; }
    }
}
