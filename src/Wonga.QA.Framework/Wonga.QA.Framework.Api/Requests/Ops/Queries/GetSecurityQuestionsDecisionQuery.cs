using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
    /// <summary> Wonga.Ops.Queries.GetSecurityQuestionsDecision </summary>
    [XmlRoot("GetSecurityQuestionsDecision")]
    public partial class GetSecurityQuestionsDecisionQuery : ApiRequest<GetSecurityQuestionsDecisionQuery>
    {
        public Object FirstSecurityQuestionExternalId { get; set; }
        public Object SecondSecurityQuestionExternalId { get; set; }
    }
}
