using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Wonga.QA.Framework.Core;
using System.Xml.Linq;
using System.Linq.Expressions;
using Wonga.QA.Framework.UI.Mappings.Pages.Wb;

namespace Wonga.QA.Framework.UI.Mappings
{

    /// <summary>
    /// Class for mapping the mapper classes instance fields with values from XML
    /// </summary>
    public class XmlMapper
    {
        private readonly XmlDocument _xmlDocument = new XmlDocument();
        private readonly XmlDocument _baseXmlDocument = new XmlDocument();
        private string _xmlFile;
        private string _baseXmlFile;

        public XmlMapper(string baseXmlFile, string xmlFile)
        {
            _xmlFile = xmlFile;
            _baseXmlFile = baseXmlFile;
            _xmlDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(_xmlFile));
            if(!string.IsNullOrEmpty(_baseXmlFile))
                _baseXmlDocument.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream(_baseXmlFile));
        }

        private String GetFieldValue(List<string> parents, String fieldName)
        {
            string path = "//";
            string myValueResult = null;

            foreach (string parent in parents)
            {
                path += parent + "/";
            }
            path += fieldName;
            var node = _xmlDocument.DocumentElement.SelectSingleNode(path) 
                ?? _baseXmlDocument.DocumentElement.SelectSingleNode(path);
            myValueResult = node == null ? null : node.InnerText;
            
            if (String.IsNullOrEmpty(myValueResult))
            {
                Trace.TraceWarning("No mapping for xml element {0} in file {1}", path, _xmlFile);
            }
            return myValueResult;
        }

        public object GetValues(object obj, List<string> parents)
        {
            var properties = obj.GetType().GetProperties().Where(x => !x.GetGetMethod().IsStatic).ToList(); ;
            parents = parents ?? new List<string>();
            parents.Add(obj.GetType().Name);
            foreach(var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, GetFieldValue(parents, property.Name), null);
                    continue;
                }

                var classValue = property.GetValue(obj, null);
                classValue = Activator.CreateInstance(property.PropertyType);
                property.SetValue(obj, GetValues(classValue, parents.ToList()), null);
            }
            return obj;
        }
    }

    //public class TestClass
    //{
    //    [MbUnit.Framework.Test]
    //    public void Test()
    //    {
    //        Elements x = Ui.Get;
    //        x.EligibilityQuestionsPage = new EligibilityQuestionsPage();

    //        Content c = Content.Get(new CultureInfo("en"));
    //    }
    //}

}
