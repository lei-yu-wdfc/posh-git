using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.CommonApi;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ivr
{
    public class IvrRequests
    {
        private static readonly dynamic CustomerDetails = Drive.Data.Comms.Db.CustomerDetails;
        private const String password = "Passw0rd";
        private const String requestedActionLiveLoan = "LiveLoan";

        [Test, AUT(AUT.Uk), JIRA("UK-1590"), Pending]
        public void LiveLoanRequestSuccessTest()
        {
            Guid guid = new Guid("6D8382B9-26CA-4F60-BF47-4905B041F17A");
            var selectedCustomer = Do.Until(() => CustomerDetails.Find(CustomerDetails.AccountId == guid));
            String email = selectedCustomer.Email;
            String dateOfBirth = selectedCustomer.DateOfBirth.ToString("dd/MM/yyyy");
            String areaCode = selectedCustomer.MobilePhone.Substring(0, 5);
            String phoneNumber = selectedCustomer.MobilePhone.Substring(5, 6);

            var request = new AccountInformationRequest();
            request.Authentication.LoginName = email;
            request.Authentication.Password = password;
            request.Directives.RequestedAction = requestedActionLiveLoan;
            request.Applicant.DateOfBirth = dateOfBirth;
            request.Applicant.MobilePhoneNumber.AreaCode = areaCode;
            request.Applicant.MobilePhoneNumber.Number = phoneNumber;

            var response = Drive.CommonApi.Commands.Post(request);

            Assert.IsNotNull(response);
            Assert.IsEmpty(response.Values["Error"]);
            Assert.AreEqual(dateOfBirth, response.Values["DateOfBirth"].Single());
            Assert.AreEqual(areaCode, response.Values["MobilePhoneSTDCode"].Single());
            Assert.AreEqual(phoneNumber, response.Values["MobilePhoneNumber"].Single());
            Assert.AreEqual(selectedCustomer.Forename, response.Values["Forename"].Single());
            Assert.AreEqual(selectedCustomer.Surname, response.Values["Surname"].Single());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1590"), Pending]
        public void LiveLoanRequestErrorAgrementInfoNotFountTest()
        {
            Guid guid = new Guid("DA8F7047-2DAA-4A59-92A9-C75B0CD509ED");
            var selectedCustomer = Do.Until(() => CustomerDetails.Find(CustomerDetails.AccountId == guid));
            String email = selectedCustomer.Email;
            String dateOfBirth = selectedCustomer.DateOfBirth.ToString("dd/MM/yyyy");
            String areaCode = selectedCustomer.MobilePhone.Substring(0, 5);
            String phoneNumber = selectedCustomer.MobilePhone.Substring(5, 6);

            var request = new AccountInformationRequest();
            request.Authentication.LoginName = email;
            request.Authentication.Password = password;
            request.Directives.RequestedAction = requestedActionLiveLoan;
            request.Applicant.DateOfBirth = dateOfBirth;
            request.Applicant.MobilePhoneNumber.AreaCode = areaCode;
            request.Applicant.MobilePhoneNumber.Number = phoneNumber;

            var response = Drive.CommonApi.Commands.Post(request);

            Assert.IsNotNull(response);
            const String errorMessage = "Empty Response Message or Error";
            Assert.AreEqual(errorMessage, response.Values["Message"].Single());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1590"), Pending]
        public void LiveLoanRequestErrorCustomerNotFountTest()
        {
            const String dateOfBirth = "10/10/1980";
            const String areaCode = "07221";
            const String phoneNumber = "12346";
            const String email = "some_wrong_email_address@wonga.com";

            var request = new AccountInformationRequest();
            request.Authentication.LoginName = email;
            request.Authentication.Password = password;
            request.Directives.RequestedAction = requestedActionLiveLoan;
            request.Applicant.DateOfBirth = dateOfBirth;
            request.Applicant.MobilePhoneNumber.AreaCode = areaCode;
            request.Applicant.MobilePhoneNumber.Number = phoneNumber;

            var response = Drive.CommonApi.Commands.Post(request);

            Assert.IsNotNull(response);
            const String errorMessage = "No Customer Matched with the information supplied.";
            Assert.AreEqual(errorMessage, response.Values["Message"].Single());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1590"), Pending]
        public void TakePaymentStoreCardTest()
        {
            const String login = "login";
            //const String password = "password";
            const String environment = "TRIAL";
            const String requestedAction = "TakePayment";
            const String referenceDate = "22/04/2010";
            const String customerReference = "6D8382B9-26CA-4F60-BF47-4905B041F17A";
            const String currency = "GBP";
            const String authorisationAmount = "60.45";
            const String cardType = "Visa Debit";
            //const String cardHolderName = "name";
            const String cardNumber = "1111111111111234";
            const String expiryDate = "0112";

            var request = new AccountInformationRequest();
            request.Authentication.LoginName = login;
            request.Directives.Environment = environment;
            request.Directives.RequestedAction = requestedAction;
            request.Directives.ReferenceDate = referenceDate;
            request.Directives.CustomerReference = customerReference;
            request.CardPaymentDetails.Currency = currency;
            request.CardPaymentDetails.AuthorisationAmount = authorisationAmount;
            request.CardPaymentDetails.RepaymentCardDetails.CardType = cardType;
            request.CardPaymentDetails.RepaymentCardDetails.CardNumber = cardNumber;
            request.CardPaymentDetails.RepaymentCardDetails.ExpiryDate = expiryDate;
            request.AgreementsToCredit.Agreement.Reference = customerReference;
            request.AgreementsToCredit.Agreement.Currency = currency;
            request.AgreementsToCredit.Agreement.Amount = authorisationAmount;

            var response = Drive.CommonApi.Commands.Post(request);

            Assert.IsNotNull(response);

            Assert.AreEqual("Error", response.Values["Outcome"].Single());
            Assert.AreEqual("Could not process query", response.Values["Message"].Single());
        }

        [Test, AUT(AUT.Uk), JIRA("UK-1590"), Pending]
        public void TakePaymentPrimaryCardTest()
        {
            const String login = "login";
            const String environment = "TRIAL";
            const String requestedAction = "TakePayment";
            const String referenceDate = "22/04/2010";
            const String customerReference = "6D8382B9-26CA-4F60-BF47-4905B041F17A";
            const String currency = "GBP";
            const String authorisationAmount = "60.45";

            var request = new AccountInformationRequest();
            request.Authentication.LoginName = login;
            request.Authentication.Password = password;
            request.Directives.Environment = environment;
            request.Directives.RequestedAction = requestedAction;
            request.Directives.ReferenceDate = referenceDate;
            request.Directives.CustomerReference = customerReference;
            request.CardPaymentDetails.Currency = currency;
            request.CardPaymentDetails.AuthorisationAmount = authorisationAmount;
            request.AgreementsToCredit.Agreement.Reference = customerReference;
            request.AgreementsToCredit.Agreement.Currency = currency;
            request.AgreementsToCredit.Agreement.Amount = authorisationAmount;
            request.CardPaymentDetails.RepaymentCardDetails = null;

            var response = Drive.CommonApi.Commands.Post(request);

            Assert.IsNotNull(response);
            Assert.AreEqual("Error", response.Values["Outcome"].Single());
            Assert.AreEqual("Could not process query", response.Values["Message"].Single());
        }
    }
}