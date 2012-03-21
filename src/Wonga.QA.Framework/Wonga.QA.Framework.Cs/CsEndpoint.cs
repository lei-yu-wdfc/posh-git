using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Cs
{
    //TODO refactor with ApiEndpoint
    public class CsEndpoint
    {
        private Uri _endpoint;

        public CsEndpoint(Uri endpoint)
        {
            _endpoint = endpoint;
        }

        public CsResponse Post(CsRequest request)
        {
            return Post(new[] { request });
        }

        public CsResponse Post(IEnumerable<CsRequest> requests)
        {
            StringBuilder builder = new StringBuilder().AppendLine("<Messages xmlns=\"http://www.wonga.com/api/3.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            foreach (CsRequest request in requests)
                builder.AppendLine(request.ToString());
            return Post(builder.AppendLine("</Messages>").ToString());
        }

        public CsResponse Post(String body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endpoint);
            request.Method = "POST";

            request.Headers.Add("Authorization", Get.GetCsAuthorization().ToString());
            request.Headers.Add("SalesForceUserName", Get.GetEmail());

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(body);
            Trace.WriteLine(Get.Indent(body), GetType().FullName);

            return new CsResponse(request);
        }
    }
}
