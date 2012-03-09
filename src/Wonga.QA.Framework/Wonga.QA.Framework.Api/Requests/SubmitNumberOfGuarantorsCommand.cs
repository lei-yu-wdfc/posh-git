using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.SubmitNumberOfGuarantors </summary>
    [XmlRoot("SubmitNumberOfGuarantors")]
    public partial class SubmitNumberOfGuarantorsCommand : ApiRequest<SubmitNumberOfGuarantorsCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object NumberOfGuarantors { get; set; }
    }
}
