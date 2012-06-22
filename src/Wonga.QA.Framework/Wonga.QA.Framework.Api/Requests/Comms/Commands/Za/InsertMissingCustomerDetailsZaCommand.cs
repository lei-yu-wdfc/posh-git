using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.InsertMissingCustomerDetailsZa </summary>
    [XmlRoot("InsertMissingCustomerDetailsZa")]
    public partial class InsertMissingCustomerDetailsZaCommand : ApiRequest<InsertMissingCustomerDetailsZaCommand>
    {
        public Object AccountId { get; set; }
        public Object Gender { get; set; }
        public Object HomePhone { get; set; }
        public Object WorkPhone { get; set; }
    }
}
