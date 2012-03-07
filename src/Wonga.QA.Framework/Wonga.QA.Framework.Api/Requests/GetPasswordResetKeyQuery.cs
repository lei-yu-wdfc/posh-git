using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Ops.Queries.GetPasswordResetKey </summary>
    [XmlRoot("GetPasswordResetKey")]
    public partial class GetPasswordResetKeyQuery : ApiRequest<GetPasswordResetKeyQuery>
    {
        public Object PwdResetKey { get; set; }
    }
}
