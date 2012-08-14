using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk
{
    /// <summary> Wonga.Comms.Commands.PayLater.Uk.SavePayLaterCustomerDetails </summary>
    [XmlRoot("SavePayLaterCustomerDetails")]
    public partial class SavePayLaterCustomerDetailsPayLaterUkCommand : ApiRequest<SavePayLaterCustomerDetailsPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object DateOfBirth { get; set; }
        public Object Title { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MobilePhone { get; set; }
        public Object Email { get; set; }
    }
}
