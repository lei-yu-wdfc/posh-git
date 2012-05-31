using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Za.SavePayUResponse </summary>
    [XmlRoot("SavePayUResponse")]
    public partial class SavePayUResponseZaCommand : ApiRequest<SavePayUResponseZaCommand>
    {
        public Object ApplicationId { get; set; }
        public Object MerchantReferenceNumber { get; set; }
        public Object TransactionAmount { get; set; }
        public Object RawRequestResponse { get; set; }
    }
}
