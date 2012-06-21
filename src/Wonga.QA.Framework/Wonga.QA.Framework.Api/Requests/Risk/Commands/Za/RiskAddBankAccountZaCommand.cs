using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Za
{
    /// <summary> Wonga.Risk.Commands.Za.RiskAddBankAccount </summary>
    [XmlRoot("RiskAddBankAccount")]
    public partial class RiskAddBankAccountZaCommand : ApiRequest<RiskAddBankAccountZaCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object AccountNumber { get; set; }
    }
}
