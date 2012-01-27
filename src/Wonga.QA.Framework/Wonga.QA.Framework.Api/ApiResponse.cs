using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public class ApiResponse
    {
        public HttpWebRequest Request { get; set; }
        public HttpWebResponse Response { get; set; }

        public String Body { get; set; }
        public Int32 Status { get; set; }
        public XElement Root { get; set; }
        public ILookup<String, String> Values { get; set; }

        public ApiResponse(HttpWebRequest request)
        {
            Request = request;

            Exception exception = null;

            try
            {
                Response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                    throw;
                Response = (HttpWebResponse)e.Response;
                exception = e;
            }
            
            Status = (Int32)Response.StatusCode;

            using (StreamReader reader = new StreamReader(Response.GetResponseStream()))
                Body = reader.ReadToEnd();
            Trace.WriteLine(Data.Indent(Body), GetType().FullName);

            try
            {
                Root = XDocument.Parse(Body).Root;
            }
            catch (XmlException e)
            {
                throw exception ?? e;
            }

            if (Root.Name.LocalName == "Messages")
            {
                String[] errors = GetErrors();
                if (errors.Any())
                {
                    if (errors.Contains("Ops_RequestXmlInvalid"))
                        throw new XsdException(errors, exception);
                    throw new ApiException(errors, exception);
                }
            }
            else
                throw new XmlException(Root.Name.LocalName, exception);

            if (exception != null)
                throw exception;

            Values = Root.Descendants().Where(e => !e.HasElements).ToLookup(e => e.Name.LocalName, e => e.Value);
        }

        public String[] GetErrors()
        {
            XNamespace ns = Root.GetDefaultNamespace();
            return Root.Descendants(ns.GetName("Error")).Select(e => e.Element(ns.GetName("Name")).Value).ToArray();
        }
    }
}
