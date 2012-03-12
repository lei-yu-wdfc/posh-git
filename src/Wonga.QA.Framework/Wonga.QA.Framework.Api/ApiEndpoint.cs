using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Schema;
using Wonga.QA.Framework.Core;

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
            StringBuilder builder = new StringBuilder().AppendLine("<Messages xmlns=\"http://www.wonga.com/api/3.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            foreach (ApiRequest request in requests)
                builder.AppendLine(request.ToString());
            return Post(builder.AppendLine("</Messages>").ToString());
        }

        public ApiResponse Post(String body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endpoint);
            request.Method = "POST";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(body);
            Trace.WriteLine(Data.Indent(body), GetType().FullName);

            return new ApiResponse(request);
        }

        public XmlSchema GetShema()
        {
            XmlSchema schema = XmlSchema.Read(new StringReader(new WebClient().DownloadString(Data.GetSchema(_endpoint))), (s, a) => { throw a.Exception; });
            XmlSchemaSet set = new XmlSchemaSet();
            set.Add(schema);
            set.Compile();
            return schema;
        }
    }
}
