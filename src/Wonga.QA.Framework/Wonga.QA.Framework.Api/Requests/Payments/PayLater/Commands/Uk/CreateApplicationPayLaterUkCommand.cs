using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk
{
    /// <summary> Wonga.Payments.PayLater.Commands.Uk.CreatePaylaterApplication </summary>
    [XmlRoot("CreatePaylaterApplication")]
    public partial class CreateApplicationPayLaterUkCommand : ApiRequest<CreateApplicationPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object MerchantId { get; set; }
        public Object MerchantReference { get; set; }
        public Object MerchantOrderId { get; set; }
        public Object TotalAmount { get; set; }
        public Object Currency { get; set; }
        public Object PostCode { get; set; }
    }
}
