using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture]
    public class FinancialAssessmentIncomeTest : UiTest
    {
        [Test, AUT(AUT.Uk), Category(TestCategories.SmokeTest), Pending("Financial Assessment")]
        public void PrepopulatedNameCheck()
        {
        }
    }
}
