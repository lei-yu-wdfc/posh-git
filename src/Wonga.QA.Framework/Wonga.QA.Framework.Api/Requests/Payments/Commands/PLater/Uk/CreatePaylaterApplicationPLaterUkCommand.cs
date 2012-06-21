using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.PLater.Uk
{
    /// <summary> Wonga.Payments.Commands.PLater.Uk.CreatePaylaterApplication </summary>
    [XmlRoot("CreatePaylaterApplication")]
    public partial class CreatePaylaterApplicationPLaterUkCommand : ApiRequest<CreatePaylaterApplicationPLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object MerchantId { get; set; }
        public Object TotalAmount { get; set; }
    }
}
