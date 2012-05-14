using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.GetLoanExtensionAgreement </summary>
    [XmlRoot("GetLoanExtensionAgreement")]
    public partial class GetLoanExtensionAgreementQuery : ApiRequest<GetLoanExtensionAgreementQuery>
    {
        public Object ExtensionId { get; set; }
    }
}
