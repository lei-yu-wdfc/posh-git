using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Wb.Uk
{
    /// <summary> Wonga.Risk.Commands.Wb.Uk.RiskAddBankAccount </summary>
    [XmlRoot("RiskAddBankAccount")]
    public partial class RiskAddBankAccountWbUkCommand : ApiRequest<RiskAddBankAccountWbUkCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object AccountNumber { get; set; }
    }
}
