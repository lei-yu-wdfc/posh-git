using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Schema;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.CommonApi
{
    public class CommonApiEndpoint
    {
        private Uri _endpoint;

        public CommonApiEndpoint(Uri endpoint)
        {
            _endpoint = endpoint;
        }

        public CommonApiResponse Post(CommonApiRequest request)
        {
            return Post(request.Serialize());
        }

        public CommonApiResponse Post(String body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_endpoint);
            request.Method = "POST";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                writer.Write(body);
            Trace.WriteLine(Get.Indent(body), GetType().FullName);

            return new CommonApiResponse(request);
        }

        public XmlSchema GetShema()
        {
            XmlSchema schema = XmlSchema.Read(new StringReader(new WebClient().DownloadString(Get.GetSchema(_endpoint))), (s, a) => { throw a.Exception; });
            XmlSchemaSet set = new XmlSchemaSet();
            set.Add(schema);
            set.Compile();
            return schema;
        }
    }
}
