using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Wb.RiskAddBankAccount </summary>
    [XmlRoot("RiskAddBankAccount")]
    public partial class RiskAddBankAccountWbCommand : ApiRequest<RiskAddBankAccountWbCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object AccountNumber { get; set; }
    }
}
