using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Schema;

namespace Wonga.QA.Framework.Api
{
    public class ApiEndpoint
    {
        private Uri _endpoint;

        public ApiEndpoint(Uri endpoint)
        {
            _endpoint = endpoint;
        }

        public ApiResponse Post(ApiRequest request)
        {
            return Post(new[] { request });
        }

        public ApiResponse Post(IEnumerable<ApiRequest> requests)
        {
            var builder = new StringBuilder().AppendLine("<Messages xmlns=\"http://www.wonga.com/api/3.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            foreach (var request in requests)
                builder.AppendLine(request.ToString());
            return Post(builder.AppendLine("</Messages>").ToString());
        }

        public ApiResponse Post(String body)
        {
            var request = (HttpWebRequest)WebRequest.Create(_endpoint);
            request.Method = "POST";

            using (var writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(body);
            Trace.WriteLine(Core.Get.Indent(body), GetType().FullName);

            return new ApiResponse(request);
        }

        public String Get(Uri uri = null)
        {
            using (var client = new WebClient())
                return client.DownloadString(uri ?? _endpoint);
        }

        public XmlSchema GetShema()
        {
            var schema = XmlSchema.Read(new StringReader(Get(Core.Get.GetSchema(_endpoint))), (s, a) => { throw a.Exception; });
            var set = new XmlSchemaSet();
            set.Add(schema);
            set.Compile();
            return schema;
        }
    }
}
