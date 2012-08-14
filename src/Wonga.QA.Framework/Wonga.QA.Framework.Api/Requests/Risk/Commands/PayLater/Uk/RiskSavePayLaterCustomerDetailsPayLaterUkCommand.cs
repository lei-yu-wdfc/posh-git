using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    /// <summary> Wonga.Risk.Commands.PayLater.Uk.RiskSavePayLaterCustomerDetails </summary>
    [XmlRoot("RiskSavePayLaterCustomerDetails")]
    public partial class RiskSavePayLaterCustomerDetailsPayLaterUkCommand : ApiRequest<RiskSavePayLaterCustomerDetailsPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object Email { get; set; }
        public Object MobilePhone { get; set; }
        public Object DateOfBirth { get; set; }
    }
}
