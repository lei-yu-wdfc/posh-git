using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Journey;
using Wonga.QA.Framework.Mobile.Ui.Pages;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.Framework.Mobile
{
    public static class JourneyFactory
    {
        public static BaseL0Journey GetL0Journey(BasePageMobile homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaMobileL0Journey(homePage);
                case AUT.Uk:
                    return new UkMobileL0Journey(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
        public static BaseLnJourney GetLnJourney(BasePageMobile homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaMobileLnJourney(homePage);
                case AUT.Uk:
                    return new UkMobileLnJourney(homePage);

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
