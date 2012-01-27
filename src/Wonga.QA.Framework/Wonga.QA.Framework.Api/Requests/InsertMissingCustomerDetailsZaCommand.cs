using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("InsertMissingCustomerDetailsZa")]
    public class InsertMissingCustomerDetailsZaCommand : ApiRequest<InsertMissingCustomerDetailsZaCommand>
    {
        public Object AccountId { get; set; }
        public Object Gender { get; set; }
        public Object HomePhone { get; set; }
        public Object WorkPhone { get; set; }
    }
}
