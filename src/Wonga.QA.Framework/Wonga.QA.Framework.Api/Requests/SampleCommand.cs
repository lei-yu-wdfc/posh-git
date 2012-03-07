using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.SampleMessage </summary>
    [XmlRoot("SampleMessage")]
    public partial class SampleCommand : ApiRequest<SampleCommand>
    {
        public Object HelloWorld { get; set; }
    }
}
