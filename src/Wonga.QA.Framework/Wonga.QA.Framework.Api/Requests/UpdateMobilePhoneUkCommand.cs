using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Uk.UpdateMobilePhone </summary>
    [XmlRoot("UpdateMobilePhone")]
    public partial class UpdateMobilePhoneUkCommand : ApiRequest<UpdateMobilePhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
    }
}
