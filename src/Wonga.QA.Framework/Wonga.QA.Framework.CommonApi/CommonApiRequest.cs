using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.CommonApi.Exceptions;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.CommonApi
{
    public abstract class CommonApiRequest
    {
        public string Serialize()
        {
            var xmlSerializer = new XmlSerializer(this.GetType());
            string outputString;
            using (var writer = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(writer, this);
                outputString = writer.ToString();
            }
            return outputString;
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
