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
        public static IL0ConsumerJourney GetL0Journey(BasePage homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaL0Journey(homePage);
                case AUT.Ca:
                    return new CaL0Journey(homePage);
                case AUT.Uk:
                    return new UkL0Journey(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
