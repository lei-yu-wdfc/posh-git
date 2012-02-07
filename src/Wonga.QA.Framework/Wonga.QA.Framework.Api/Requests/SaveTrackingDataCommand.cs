using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveTrackingData")]
    public partial class SaveTrackingDataCommand : ApiRequest<SaveTrackingDataCommand>
    {
    }
}
