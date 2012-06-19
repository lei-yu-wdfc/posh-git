using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Journey
{
    public static class JourneyFactory
    {
        public static IL0MobileConsumerJourney GetL0Journey(BasePageMobile homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaMobileL0Journey(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
        //public static ILnConsumerJourney GetLnJourney(BasePage homePage)
        //{
            
        //}
        
    }
}
