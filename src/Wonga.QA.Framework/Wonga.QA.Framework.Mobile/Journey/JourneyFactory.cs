using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Journey;
using Wonga.QA.Framework.Mobile.Ui.Pages;

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
                default:
                    throw new NotImplementedException();
            }
        }
        public static ILnConsumerJourney GetLnJourney(BasePageMobile homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaMobileLnJourney(homePage);

                default:
                    throw new NotImplementedException();
            }
        }

    }
}
