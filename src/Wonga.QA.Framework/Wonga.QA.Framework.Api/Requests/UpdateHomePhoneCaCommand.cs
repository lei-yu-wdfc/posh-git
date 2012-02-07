using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("UpdateHomePhone")]
    public partial class UpdateHomePhoneCaCommand : ApiRequest<UpdateHomePhoneCaCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
