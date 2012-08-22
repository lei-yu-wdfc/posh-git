using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.Ui.Validators;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment
{
    public class FARejectedPage : BasePage, IDecisionPage
    {
        public FARejectedPage(UiClient client, Validator validator = null)
            : base(client, validator)
        {
            if (!base.Url.Contains("/rejected"))
            {
                throw new Exception();
            }
        }
    }
}
