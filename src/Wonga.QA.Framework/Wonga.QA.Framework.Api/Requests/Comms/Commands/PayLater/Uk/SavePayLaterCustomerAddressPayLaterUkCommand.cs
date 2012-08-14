using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk
{
    /// <summary> Wonga.Comms.Commands.PayLater.Uk.SavePayLaterCustomerAddress </summary>
    [XmlRoot("SavePayLaterCustomerAddress")]
    public partial class SavePayLaterCustomerAddressPayLaterUkCommand : ApiRequest<SavePayLaterCustomerAddressPayLaterUkCommand>
    {
        public Object AddressId { get; set; }
        public Object AccountId { get; set; }
        public Object Flat { get; set; }
        public Object HouseNumber { get; set; }
        public Object Street { get; set; }
        public Object Town { get; set; }
        public Object Postcode { get; set; }
    }
}
