using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Tests.Ui
{
    class L0JourneyTests : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca)]
        public void L0JourneyInvalidPostcodeShouldCauseWarningMessage()
        {
            var journey = new Journey(Client.Home());
            var addressPage = journey.ApplyForLoan(200, 10)
                                      .FillPersonalDetails("test:EmployedMask")
                                      .CurrentPage as AddressDetailsPage;
            switch (Config.AUT)
            {
                case AUT.Za:
                    addressPage.PostCode = "qqqq";
                    break;
                case AUT.Ca:
                    addressPage.PostCode = "111111";
                    break;
            }
            addressPage.Street = "Asd"; //to lost focus
            Assert.IsTrue(addressPage.IsPostcodeWarningOccurred());

        }
    }
}
