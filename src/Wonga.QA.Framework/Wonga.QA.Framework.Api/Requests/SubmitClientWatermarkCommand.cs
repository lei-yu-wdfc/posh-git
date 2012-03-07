using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.SubmitClientWatermark </summary>
    [XmlRoot("SubmitClientWatermark")]
    public partial class SubmitClientWatermarkCommand : ApiRequest<SubmitClientWatermarkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object ClientIPAddress { get; set; }
        public Object BlackboxData { get; set; }
    }
}
