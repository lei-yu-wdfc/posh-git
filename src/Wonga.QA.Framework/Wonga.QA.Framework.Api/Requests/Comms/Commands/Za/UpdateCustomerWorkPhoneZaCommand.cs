using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateCustomerWorkPhoneZa </summary>
    [XmlRoot("UpdateCustomerWorkPhoneZa")]
    public partial class UpdateCustomerWorkPhoneZaCommand : ApiRequest<UpdateCustomerWorkPhoneZaCommand>
    {
        public Object AccountId { get; set; }
        public Object WorkPhone { get; set; }
    }
}
