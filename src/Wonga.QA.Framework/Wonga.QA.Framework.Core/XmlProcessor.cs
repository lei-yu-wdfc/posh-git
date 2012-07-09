using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Wonga.QA.Framework.Core
{
    public class XmlProcessor
    {
        private string _filepath;
        private XDocument _xDocument;
        private static Regex regex;
        private string _specialString = "%%";

        public XmlProcessor()
        {
            regex = new Regex(string.Format("{0}[a-zA-Z0-9+-://@]+{0}", _specialString));
        }

        public XDocument LoadFromFile(string filepath)
        {
            _filepath = filepath;
            _xDocument = XDocument.Load(filepath);
            return GetProcessedXDoc();
        }

        public XDocument LoadFromStream(Stream stream)
        {
            _xDocument = XDocument.Load(stream);
            return GetProcessedXDoc();
        }

        private string StripSpecialStrings(string val)
        {
            return val.Replace(_specialString, "");
        }

        private string DecorateSpecialStrings(string val)
        {
            return string.Format("{0}{1}{0}", _specialString, val);
        }

        public XDocument LoadFromString(string val)
        {
            _xDocument = XDocument.Load(new StringReader(val));
            return GetProcessedXDoc();
        }
        
        /// <summary>
        /// Replaces all %% tokens with their xpath values
        /// </summary>
        /// <returns></returns>
        private XDocument GetProcessedXDoc()
        {
            var matches = (from e in _xDocument.Descendants()
                           where IsMatch(e.Value) 
                           && e.Descendants().Count() == 0
                           select e).ToList();
            foreach (var match in matches)
            {
                match.Value = ProcessNode(match);
            }
            return _xDocument;
        }

        /// <summary>
        /// Takes a node and returns inner value
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string ProcessNode(XElement node)
        {
            if (!IsMatch(node.Value))
                return node.Value;
            foreach(var token in ReadToken(node))
            {
                node.Value = node.Value.Replace(DecorateSpecialStrings(token), ProcessNode(_xDocument.XPathSelectElement(StripSpecialStrings(token))));
            }
            return node.Value;
        }

        public bool IsMatch(string value)
        {
            return regex.IsMatch(value);
        }

        public IEnumerable<string> ReadToken(XElement value)
        {
            foreach(Group group in regex.Matches(value.Value))
            {
                foreach(Capture capture in group.Captures)
                {
                    yield return StripSpecialStrings(capture.Value);
                }
            }
        }
    }
}