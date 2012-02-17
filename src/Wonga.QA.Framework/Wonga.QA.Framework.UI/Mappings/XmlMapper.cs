using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class XmlMapper
    {
        private readonly XDocument _xmlDocument;

        internal XmlMapper()
        {
            _xmlDocument = XDocument.Load(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "Wonga.QA.Framework.UI.SeleniumMap.xml"));
        }

        internal String GetValue<T>(Expression<Func<T>> containerName)
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
            var myValueResult =_xmlDocument.Descendants(Config.AUT.ToString()).Descendants(containerName).Descendants(fieldName).Select
                    (containerFieldValue => containerFieldValue.Value).Select(p=>p.ToString()).SingleOrDefault();

            if (String.IsNullOrEmpty(myValueResult))
            {
                throw new NotImplementedException(
                    String.Format("SELENIUM_XML_MAPPING_ERROR -> The value for pair << {0} - {1} >> in SeleniumMap.XML for << {2} >> is missing - PLEASE REVIEW", containerName,
                                  fieldName,Config.AUT.ToString()));
            }
            return myValueResult;
        }
    }

    //internal class TestClass
    //{
    //    [MbUnit.Framework.Test]
    //    internal void Test()
    //    {
    //        WbElements x = new WbElements();
    //        x.WbEligibilityQuestionsPage = new EligibilityQuestionsPage();
    //        x.WbEligibilityQuestionsPage.FormId = x.XmlMapper.GetValue(() => x.WbEligibilityQuestionsPage.FormId);
    //    }
    //}
}
