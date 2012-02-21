using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
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
        private readonly XDocument _xmlDocument;
        private string _xmlFile;
        public XmlMapper()
        {
            _xmlFile = string.Format("Wonga.QA.Framework.UI.Mappings.Xml.{0}.xml", Config.AUT);
            _xmlDocument = XDocument.Load(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(_xmlFile));
        }

        public String GetValue<T>(Expression<Func<T>> containerName)
        {
            /*NOTE: This strange and simple method uses reflection to get the TYPE and FIELD names given a field as input.
             * You need to have the EXACT NAMES inside the XML.
             * If in the mapper class we have WbEligibilityQuestionsPage.FormID => in the XML we need:
             * <WbEligibilityQuestionsPage>
             *      <FormId>THE_VALUE</FormId>
             * </WbEligibilityQuestionsPage>
             */
            var typeFieldName = ((MemberExpression) containerName.Body).Member.Name;
            var declaringTypeName = ((MemberExpression) containerName.Body).Member.DeclaringType.Name;
            return GetFieldValue(declaringTypeName, typeFieldName);
        }

        private String GetFieldValue(String containerName, String fieldName)
        {
            var myValueResult =_xmlDocument.Descendants(containerName).Descendants(fieldName).Select
                    (containerFieldValue => containerFieldValue.Value).Select(p=>p.ToString()).SingleOrDefault();

            
            if (String.IsNullOrEmpty(myValueResult))
            {
                Trace.TraceWarning("No mapping for element {0}, {1} in file {2}", containerName, fieldName, _xmlFile);
                //throw new NotImplementedException(
                //    String.Format("SELENIUM_XML_MAPPING_ERROR -> The value for pair << {0} - {1} >> in SeleniumMap.XML for << {2} >> is missing - PLEASE REVIEW", containerName,
                //                  fieldName,Config.AUT.ToString()));
            }
            return myValueResult;
        }

        public void GetValues(object obj)
        {
            var properties = obj.GetType().GetProperties().Where(x => !x.GetGetMethod().IsStatic).ToList(); ;
            foreach(var baseClass in properties)
            {
                var className = baseClass.Name;
                var classValue = baseClass.GetValue(obj, null);
                classValue = Activator.CreateInstance(baseClass.PropertyType);
                var baseProperties = baseClass.PropertyType.GetProperties();
                foreach(var baseElement in baseProperties)
                {
                    baseElement.SetValue(classValue, GetFieldValue(className, baseElement.Name), null);
                }
                baseClass.SetValue(obj,classValue, null);
            }
        }
    }

    //public class TestClass
    //{
    //    [MbUnit.Framework.Test]
    //    public void Test()
    //    {
    //        Elements x = Elements.Get;
    //        x.WbEligibilityQuestionsPage = new EligibilityQuestionsPage();
    //        x.WbEligibilityQuestionsPage.FormId = x.XmlMapper.GetValue(() => x.WbEligibilityQuestionsPage.FormId);
    //    }
    //}
}
