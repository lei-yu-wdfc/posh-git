using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.SetPaymentCardPrimary </summary>
    [XmlRoot("SetPaymentCardPrimary")]
    public partial class SetPaymentCardPrimaryCommand : ApiRequest<SetPaymentCardPrimaryCommand>
    {
        public Object AccountId { get; set; }
        public Object PaymentCardId { get; set; }
    }
}
