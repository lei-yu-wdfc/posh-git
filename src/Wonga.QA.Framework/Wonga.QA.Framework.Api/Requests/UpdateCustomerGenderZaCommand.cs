using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("UpdateCustomerGenderZa")]
    public partial class UpdateCustomerGenderZaCommand : ApiRequest<UpdateCustomerGenderZaCommand>
    {
        public Object AccountId { get; set; }
        public Object Gender { get; set; }
    }
}
