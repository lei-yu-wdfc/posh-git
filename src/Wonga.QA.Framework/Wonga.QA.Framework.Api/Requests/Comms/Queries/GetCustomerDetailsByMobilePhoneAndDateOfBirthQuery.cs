using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
    /// <summary> Wonga.Comms.Queries.GetCustomerDetailsByMobilePhoneAndDateOfBirth </summary>
    [XmlRoot("GetCustomerDetailsByMobilePhoneAndDateOfBirth")]
    public partial class GetCustomerDetailsByMobilePhoneAndDateOfBirthQuery : ApiRequest<GetCustomerDetailsByMobilePhoneAndDateOfBirthQuery>
    {
        public Object DateOfBirth { get; set; }
        public Object MobilePhone { get; set; }
    }
}
