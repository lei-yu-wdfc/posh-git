using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Queries.GetEmploymentDetails </summary>
    [XmlRoot("GetEmploymentDetails")]
    public partial class GetEmploymentDetailsQuery : ApiRequest<GetEmploymentDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
