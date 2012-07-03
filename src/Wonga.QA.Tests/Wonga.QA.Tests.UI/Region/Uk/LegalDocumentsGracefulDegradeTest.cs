﻿using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Risk.EmploymentStatusEnum;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class LegalDocumentsGracefulDegradeTest : UiTest
    {

        /* Check L0 account setup link toggle is clicked and SECCI, T&C or explanation links are broken 
         * and a prescribed error message is displayed and that progression in the application is stopped
         * Returning to the start of a journey will clear errors and alow progression
         */
        [Test, AUT(AUT.Uk), JIRA("UKWEB-365"), MultipleAsserts, IgnorePageErrors, Pending("Development"]
        public void L0LegalDocumentErrorIsStoppedAndMessaged()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithEmail(email);

            var accountSetupPage = journeyL0.Teleport<AccountDetailsPage>() as AccountDetailsPage;

            accountSetupPage.AccountDetailsSection.Password = "Passw0rd";
            accountSetupPage.AccountDetailsSection.PasswordConfirm = "Passw0rd";
            accountSetupPage.AccountDetailsSection.SecretQuestion = "Secret question'-.";
            accountSetupPage.AccountDetailsSection.SecretAnswer = "Secret answer";

            //Turns off SECCI, T&C, Explanation to simulate content error
            var secciLink = accountSetupPage.GetSecciToggleElement();
            secciLink.SecciToggleButtonClick();

            const String errorMessage =
                "Oops. We are having technical issues and are unable to complete your application. Please try again shortly or call us on 08448 429 109";

            //Check Broken Secci
            accountSetupPage.ClickSecciLink();
            Assert.Contains(accountSetupPage.SecciPopupWindowContent(), errorMessage);
            accountSetupPage.ClosePopupWindow();

            accountSetupPage = accountSetupPage.NextClick();

            //Find error text in each
            Assert.IsTrue(accountSetupPage.IsSecciLinkVisible());
            Assert.IsTrue(accountSetupPage.IsTermsAndConditionsLinkVisible());
            Assert.IsTrue(accountSetupPage.IsExplanationLinkVisible());

            Assert.Contains(accountSetupPage.GetTermsAndConditionsTitle(), "Wonga.com Loan Conditions");
            accountSetupPage.ClosePopupWindow();

            Thread.Sleep(1000);

            Assert.Contains(accountSetupPage.GetExplanationTitle(), "Important information about your loan");
            accountSetupPage.ClosePopupWindow();

            Thread.Sleep(1000);

            

            
            //var secciLink = accountSetupPage.GetSecciToggleElement();
            //secciLink.SecciToggleButtonClick();
        }
    }
}