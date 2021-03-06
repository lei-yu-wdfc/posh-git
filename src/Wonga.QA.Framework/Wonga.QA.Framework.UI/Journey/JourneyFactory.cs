﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Journey;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;

namespace Wonga.QA.Framework.UI
{
    public static class JourneyFactory
    {
        public static BaseL0Journey GetL0Journey(BasePage homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaL0Journey(homePage);
                case AUT.Ca:
                    return new CaL0Journey(homePage);
                case AUT.Uk:
                    return new UkL0Journey(homePage);
                case AUT.Pl:
                    return new PlL0Journey(homePage);
                case AUT.Wb:
                    return new WbL0Journey(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
        public static BaseLnJourney GetLnJourney(BasePage homePage)
        {
            switch (Config.AUT)
            {
                case AUT.Za:
                    return new ZaLnJourney(homePage);
                case AUT.Ca:
                    return new CaLnJourney(homePage);
                case AUT.Uk:
                    return new UkLnJourney(homePage);
                case AUT.Wb:
                    return new WbLnJourney(homePage);
                default:
                    throw new NotImplementedException();
            }
        }
        public static BaseFALnJourney GetFaLnJourney(FinancialAssessmentPage financialAssessmentPage)
        {
            switch (Config.AUT)
            {
                case AUT.Uk:
                    return new UkFALnJourney(financialAssessmentPage);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
