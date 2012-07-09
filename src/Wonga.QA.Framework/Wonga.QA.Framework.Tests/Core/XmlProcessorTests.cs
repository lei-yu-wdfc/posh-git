using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Tests.Core
{
    [TestFixture]
    public class XmlProcessorTests
    {
        private string sampleXml = "<Configuration>" +
                                   "    <Setting>Foo</Setting>" +
                                   "    <Setting2>%%//Setting%%</Setting2>" +
                                   "    <Setting3>%%//Setting%%%%//Setting2%%</Setting3>" +
                                   "    <Setting4>%%//Setting%%_%%//Setting2%%</Setting4>" +
                                   "</Configuration>";

        [Test]
        public void WhenOneSettingDependsOnAnotherItShouldBeResolvedCorrectly()
        {
            XmlProcessor proc =new XmlProcessor();
            XDocument result = proc.LoadFromString(sampleXml);
            Assert.AreEqual("Foo", result.XPathSelectElement("//Setting2").Value);
        }

        [Test]
        public void WhenOneSettingDependsOnTwoOtherItShouldBeResolvedCorrectly()
        {
            XmlProcessor proc = new XmlProcessor();
            XDocument  result = proc.LoadFromString(sampleXml);
            Assert.AreEqual("FooFoo", result.XPathSelectElement("//Setting3").Value);
        }

        [Test]
        public void WhenOneSettingDependsOnTwoOtherAndItHasAnUnderscoreInBetweenItShouldBeResolvedCorrectly()
        {
            XmlProcessor proc = new XmlProcessor();
            XDocument result = proc.LoadFromString(sampleXml);
            Assert.AreEqual("Foo_Foo", result.XPathSelectElement("//Setting4").Value);
        }
    }
}
