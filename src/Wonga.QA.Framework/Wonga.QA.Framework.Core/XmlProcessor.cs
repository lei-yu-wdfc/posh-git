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
    internal class XmlProcessor
    {
        private XDocument _xDocument;
        private static Regex regex;
        private const string TOKENS_STRING = "%%";

        public XmlProcessor()
        {
            regex = new Regex(string.Format("{0}[a-zA-Z0-9+-://@]+{0}", TOKENS_STRING));
        }

        public XDocument LoadFromFile(string filepath)
        {
            _xDocument = XDocument.Load(filepath);
            return GetProcessedXDoc();
        }

        public XDocument LoadFromXDoc(XDocument doc)
        {
            _xDocument = doc;
            return GetProcessedXDoc();
        }

        public XDocument LoadFromStream(Stream stream)
        {
            _xDocument = XDocument.Load(stream);
            return GetProcessedXDoc();
        }

        private string StripSpecialStrings(string val)
        {
            return val.Replace(TOKENS_STRING, "");
        }

        private string DecorateSpecialStrings(string val)
        {
            return string.Format("{0}{1}{0}", TOKENS_STRING, val);
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


        public static XElement CreateNode(XDocument doc, string xpath)
        {
            return CreateNode(doc, null, xpath);
        }

        public static XElement CreateNode(XDocument doc, XElement parent, string xpath)
        {
            string[] partsOfXPath = xpath.Trim('/').Split('/');
            var regex = new Regex(@"([a-zA-Z0-9]+)");
            var nodeName = partsOfXPath.First();
            var matches = regex.Match(nodeName);
            if (string.IsNullOrEmpty(xpath))
                return parent;
            string nextNodeInXPath = matches.Groups[0].Captures[0].Value;

            // get or create the node from the name
            XElement node = parent != null ? parent.XPathSelectElement(nextNodeInXPath) : doc.XPathSelectElement(nextNodeInXPath);
            if (node == null)
            {
                node = new XElement(nextNodeInXPath);
                parent.Add(node);
            }

            // rejoin the remainder of the array as an xpath expression and recurse
            string rest = String.Join("/", partsOfXPath.Skip(1).ToArray());
            return CreateNode(doc, node, rest);
        }
    }

    public static class XExtensions
    {
        /// <summary>
        /// Get the absolute XPath to a given XElement
        /// (e.g. "/people/person[6]/name[1]/last[1]").
        /// </summary>
        public static string GetAbsoluteXPath(this XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Func<XElement, string> relativeXPath = e =>
            {
                int index = e.IndexPosition();
                string name = e.Name.LocalName;

                // If the element is the root, no index is required

                return (index == -1) ? "/" + name : string.Format
                (
                    "/{0}[{1}]",
                    name,
                    index.ToString()
                );
            };

            var ancestors = from e in element.Ancestors()
                            select relativeXPath(e);

            return string.Concat(ancestors.Reverse().ToArray()) +
                   relativeXPath(element);
        }

        /// <summary>
        /// Get the index of the given XElement relative to its
        /// siblings with identical names. If the given element is
        /// the root, -1 is returned.
        /// </summary>
        /// <param name="element">
        /// The element to get the index of.
        /// </param>
        public static int IndexPosition(this XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (element.Parent == null)
            {
                return -1;
            }

            int i = 1; // Indexes for nodes start at 1, not 0

            foreach (var sibling in element.Parent.Elements(element.Name))
            {
                if (sibling == element)
                {
                    return i;
                }

                i++;
            }

            throw new InvalidOperationException
                ("element has been removed from its parent.");
        }
    }
}