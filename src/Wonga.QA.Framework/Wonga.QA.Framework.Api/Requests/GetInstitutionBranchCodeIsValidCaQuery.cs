using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetInstitutionBranchCodeIsValid")]
    public partial class GetInstitutionBranchCodeIsValidCaQuery : ApiRequest<GetInstitutionBranchCodeIsValidCaQuery>
    {
        public Object InstitutionNumber { get; set; }
        public Object BranchCode { get; set; }
    }
}
