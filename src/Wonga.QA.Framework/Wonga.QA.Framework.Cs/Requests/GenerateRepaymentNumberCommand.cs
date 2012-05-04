using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.GenerateRepaymentNumber </summary>
    [XmlRoot("GenerateRepaymentNumber")]
    public partial class GenerateRepaymentNumberCommand : CsRequest<GenerateRepaymentNumberCommand>
    {
        public Object AccountId { get; set; }
    }
}
