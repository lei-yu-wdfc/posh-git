using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetMatchingAccounts")]
    public partial class GetMatchingAccountsQuery : ApiRequest<GetMatchingAccountsQuery>
    {
        public Object DateOfBirth { get; set; }
        public Object Title { get; set; }
        public Object Forename { get; set; }
        public Object Surname { get; set; }
        public Object MiddleName { get; set; }
        public Object HomePhone { get; set; }
        public Object MobilePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object Email { get; set; }
        public Object Postcode { get; set; }
    }
}
