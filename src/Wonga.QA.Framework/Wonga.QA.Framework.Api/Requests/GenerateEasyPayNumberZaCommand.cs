using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Za.GenerateEasyPayNumber </summary>
    [XmlRoot("GenerateEasyPayNumber")]
    public partial class GenerateEasyPayNumberZaCommand : ApiRequest<GenerateEasyPayNumberZaCommand>
    {
        public Object AccountId { get; set; }
    }
}
