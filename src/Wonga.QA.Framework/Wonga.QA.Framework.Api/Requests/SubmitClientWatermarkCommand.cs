using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SubmitClientWatermark")]
    public class SubmitClientWatermarkCommand : ApiRequest<SubmitClientWatermarkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object ClientIPAddress { get; set; }
        public Object BlackboxData { get; set; }
    }
}
