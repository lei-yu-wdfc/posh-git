using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.SaveContactPreferences </summary>
    [XmlRoot("SaveContactPreferences")]
    public partial class SaveContactPreferencesCommand : ApiRequest<SaveContactPreferencesCommand>
    {
        public Object AccountId { get; set; }
        public Object AcceptMarketingContact { get; set; }
    }
}
