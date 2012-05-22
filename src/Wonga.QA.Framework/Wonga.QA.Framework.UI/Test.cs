using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Testz()
        {
            var res = UiMap.Get.AcceptedPage;
            var content = ContentMap.Get.LoanAgreement.Header;
        }
    }
}
