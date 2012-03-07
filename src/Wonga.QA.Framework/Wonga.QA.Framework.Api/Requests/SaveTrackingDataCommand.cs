using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.SaveTrackingData </summary>
    [XmlRoot("SaveTrackingData")]
    public partial class SaveTrackingDataCommand : ApiRequest<SaveTrackingDataCommand>
    {
    }
}
