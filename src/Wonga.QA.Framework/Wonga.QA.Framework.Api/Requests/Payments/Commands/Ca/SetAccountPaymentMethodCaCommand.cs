using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Ca
{
    /// <summary> Wonga.Payments.Commands.Ca.SetAccountPaymentMethod </summary>
    [XmlRoot("SetAccountPaymentMethod")]
    public partial class SetAccountPaymentMethodCaCommand : ApiRequest<SetAccountPaymentMethodCaCommand>
    {
        public Object AccountId { get; set; }
        public Object CashoutPaymentMethod { get; set; }
    }
}
