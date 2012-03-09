using System.Net;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Cs
{
    public class CsResponse : ApiResponse
    {
        public CsResponse(HttpWebRequest request) : base(request) { }
    }
}
