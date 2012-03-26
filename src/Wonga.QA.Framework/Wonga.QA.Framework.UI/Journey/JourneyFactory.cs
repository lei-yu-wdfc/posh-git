using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
    public static class JourneyFactory
    {
        public static IConsumerJourney GetL0Journey(BasePage homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaJourney(homePage);
                case AUT.Ca:
                    return new CaJourney(homePage);
                case AUT.Uk:
                    return new UkJourney(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
