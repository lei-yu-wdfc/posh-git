using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Gallio.Framework;
using Gallio.Framework.Assertions;
using Gallio.Model;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Mappings.Sections;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    public abstract class UiTest
    {
        private TestLocal<UiClient> _uiClient = new TestLocal<UiClient>(() => GetNewClient());

        private static UiClient GetNewClient()
        {
            return new UiClient();
        }

        public UiClient Client
        {
            get { return _uiClient.Value; }
        }
        protected String _firstName;
        protected String _lastName;

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                var name = TestContext.CurrentContext.Test.Name;
                if (Client.Driver is CustomRemoteWebDriver)
                    SauceRestClient.UpdateJobPassFailStatus(((CustomRemoteWebDriver)Client.Driver).GetSessionId());
                if (!Config.Ui.RemoteMode)
                    TestLog.EmbedImage(name + ".Screen", Client.Screen());
                TestLog.AttachHtml(name + ".Source", Client.Source());
            }
            finally
            {
                Client.Dispose();
            }
        }

        public void SourceContains(string token)
        {
            Assert.IsTrue(Client.Driver.PageSource.Contains(token));
        }

        public void SourceDoesNotContain(string token)
        {
            Assert.IsFalse(Client.Driver.PageSource.Contains(token));
        }

        public void SourceContains(List<KeyValuePair<string, string>> list)
        {
            foreach (var pair in list)
            {
                try
                {
                    Assert.IsTrue(Client.Driver.PageSource.Contains(String.Format("{0}: [{1}]", pair.Key, pair.Value)));
                }
                catch (AssertionException exception)
                {
                    throw new AssertionException(String.Format("{0} - failed on {1}: {2}", exception.Message, pair.Key, pair.Value));
                }
            }
        }

        public void SourceDoesNotContain(List<KeyValuePair<string, string>> list)
        {
            foreach (var pair in list)
            {
                try
                {
                    Assert.IsFalse(Client.Driver.PageSource.Contains(String.Format("{0}: [{1}]", pair.Key, pair.Value)));
                }
                catch (AssertionException exception)
                {
                    throw new AssertionException(String.Format("{0} - failed on {1}: {2}", exception.Message, pair.Key, pair.Value));
                }
            }
        }
    }
}