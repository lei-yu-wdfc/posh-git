using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPreApprovedDirectDebitForm")]
    public class GetPreApprovedDirectDebitFormCaQuery : ApiRequest<GetPreApprovedDirectDebitFormCaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
