using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("UpdateHomePhoneZa")]
    public partial class UpdateHomePhoneZaCommand : ApiRequest<UpdateHomePhoneZaCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
