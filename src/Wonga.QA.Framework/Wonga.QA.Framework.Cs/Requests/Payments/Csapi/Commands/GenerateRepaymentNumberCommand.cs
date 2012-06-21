using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.GenerateRepaymentNumber </summary>
    [XmlRoot("GenerateRepaymentNumber")]
    public partial class GenerateRepaymentNumberCommand : CsRequest<GenerateRepaymentNumberCommand>
    {
        public Object AccountId { get; set; }
    }
}
