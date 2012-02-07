using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("DeletePaymentCard")]
    public partial class DeletePaymentCardCommand : ApiRequest<DeletePaymentCardCommand>
    {
        public Object AccountId { get; set; }
        public Object PaymentCardId { get; set; }
    }
}
