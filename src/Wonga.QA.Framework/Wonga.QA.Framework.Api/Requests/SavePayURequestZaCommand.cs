using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Za.SavePayURequest </summary>
    [XmlRoot("SavePayURequest")]
    public partial class SavePayURequestZaCommand : ApiRequest<SavePayURequestZaCommand>
    {
        public Object ApplicationId { get; set; }
        public Object MerchantReferenceNumber { get; set; }
        public Object TransactionAmount { get; set; }
        public Object RequestedOn { get; set; }
    }
}
