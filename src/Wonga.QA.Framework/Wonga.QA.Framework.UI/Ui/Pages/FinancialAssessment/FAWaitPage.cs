using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Ui.Validators;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FAWaitPage : BasePage
    {
        public FAWaitPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
        }

        public IFADecisionPage WaitFor<T>() where T : IFADecisionPage
        {
            if (typeof(T) == typeof(FAAcceptedPage))
                return Do.With.Timeout(2).Until(() => new FAAcceptedPage(Client));

            if (typeof(T) == typeof(FACounterOfferPage))
                return Do.With.Timeout(2).Until(() => new FACounterOfferPage(Client));

            if (typeof(T) == typeof(FARejectedPage))
                return Do.With.Timeout(2).Until(() => new FARejectedPage(Client));
            throw new NotImplementedException();
        }
    }
}
