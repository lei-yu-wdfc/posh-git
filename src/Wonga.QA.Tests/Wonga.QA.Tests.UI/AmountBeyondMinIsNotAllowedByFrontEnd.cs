using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;


namespace Wonga.QA.Tests.Ui
{
    class AmountBeyondMinIsNotAllowedByFrontEnd : UiTest
    {
        private static string _minAmountCa;
        private static string _setAmountCa;

        [SetUp, JIRA("QA-237")]
        public void GetDataFromDB()
        {
            _minAmountCa = "100"; // mast take from DB
            _setAmountCa = (Int32.Parse(_minAmountCa) - 1).ToString();
        }

        [Test, AUT(AUT.Ca), JIRA("QA-237"), Pending("_minAmountCa must be taken from DB")]
        public void ChangingAmountBeyondMinIsNotAllowedByFrontEnd()
        {
            var page = Client.Home();
            page.Sliders.HowMuch = _setAmountCa;
            page.Help.HelpTriggerClick(); // For page.Sliders.HowMuch lost focus
            Assert.AreEqual(page.Sliders.HowMuch, _minAmountCa);
        }
    }
}
