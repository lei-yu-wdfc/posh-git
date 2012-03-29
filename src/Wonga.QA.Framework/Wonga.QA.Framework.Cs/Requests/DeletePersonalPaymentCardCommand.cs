using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.DeletePersonalPaymentCard </summary>
    [XmlRoot("DeletePersonalPaymentCard")]
    public partial class DeletePersonalPaymentCardCommand : CsRequest<DeletePersonalPaymentCardCommand>
    {
        public Object PaymentCardId { get; set; }
    }
}
