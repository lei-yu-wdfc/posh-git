using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Journey;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
    public static class JourneyFactory
    {
        public static IL0ConsumerJourney GetL0Journey(BasePage homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    if (Config.Ui.Browser.Equals(Config.UiConfig.BrowserType.FirefoxMobile))
                        return new ZaMobileL0Journey(homePage);
                    return new ZaL0Journey(homePage);
                case AUT.Ca:
                    return new CaL0Journey(homePage);
                case AUT.Uk:
                    return new UkL0Journey(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
        public static ILnConsumerJourney GetLnJourney(BasePage homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    if (Config.Ui.Browser.Equals(Config.UiConfig.BrowserType.FirefoxMobile))
                        return new ZaMobileLnJourney(homePage);
                    return new ZaLnJourney(homePage);
                case AUT.Ca:
                    return new CaLnJourney(homePage);
                case AUT.Uk:
                    return new UkLnJourney(homePage);
                default:
                    throw new NotImplementedException();
            }
        }

        public static WbL0Journey GetL0JourneyWB(BasePage homePage)
        {
            return new WbL0Journey(homePage);
        }
    }
}
