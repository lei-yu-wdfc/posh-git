using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI.Journey
{
    abstract class BaseL0Journey : IL0ConsumerJourney
    {
        protected Dictionary<Type, Func<bool, IL0ConsumerJourney>> journey = new Dictionary<Type, Func<bool, IL0ConsumerJourney>>();

        protected bool _submit;

        protected int _amount;
        protected int _duration;

        protected String _firstName;
        protected String _lastName;
        protected String _middleName;
        protected String _title;
        protected String _motherMaidenName;
        protected String _email;
        protected String _employerName;
        protected String _mobilePhone;
        protected String _nationalId;
        protected DateTime _dateOfBirth;
        protected GenderEnum _gender;

        protected String _postCode;
        protected String _addressPeriod;

        protected String _password;

        protected String _accountNumber;
        protected String _bankPeriod;
        protected String _pin;

        protected String _cardNumber;
        protected String _cardSecurity;
        protected String _cardType;
        protected String _expiryDate;
        protected String _startDate;

        public BasePage CurrentPage { get; set; }

        public BasePage Teleport<T>()
        {
            var pageType = typeof(T);
            var currentIndex = CurrentPage == null ? 0 : journey.Keys.ToList().IndexOf(CurrentPage.GetType());
            for (int i = currentIndex; i < journey.Keys.Count; i++)
            {
                if (CurrentPage.GetType() == pageType)
                {
                    if (!_submit)
                    {
                        journey.ElementAt(i).Value.Invoke(_submit);
                    }
                    return CurrentPage;
                }
                else
                {
                    journey.ElementAt(i).Value.Invoke(true);
                }
            }
            return CurrentPage;
        }

        public abstract IL0ConsumerJourney ApplyForLoan(bool submit = true);

        public abstract IL0ConsumerJourney FillPersonalDetails(bool submit = true);

        public abstract IL0ConsumerJourney FillAddressDetails(bool submit = true);

        public abstract IL0ConsumerJourney FillAccountDetails(bool submit = true);

        public abstract IL0ConsumerJourney FillBankDetails(bool submit = true);

        public virtual IL0ConsumerJourney FillCardDetails(bool submit = true)
        {
            throw new NotImplementedException();
        }

        public abstract IL0ConsumerJourney WaitForAcceptedPage(bool submit = true);

        public abstract IL0ConsumerJourney WaitForDeclinedPage(bool submit = true);

        public abstract IL0ConsumerJourney FillAcceptedPage(bool submit = true);

        public abstract IL0ConsumerJourney GoToMySummaryPage(bool submit = true);

        public virtual IL0ConsumerJourney IgnoreAcceptingLoanAndReturnToHomePageAndLogin(bool submit = true)
        {
            throw new NotImplementedException();
        }

        public virtual IL0ConsumerJourney AnswerEligibilityQuestions(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }
        public virtual IL0ConsumerJourney EnterBusinessDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }
       public virtual IL0ConsumerJourney DeclineAddAdditionalDirector(bool submit = true)
       {
           throw new NotImplementedException(message: "Used only on Wb");
       }

        public virtual IL0ConsumerJourney AddAdditionalDirector(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney EnterBusinessBankAccountDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney EnterBusinessDebitCardDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }
        public virtual IL0ConsumerJourney WaitForApplyTermsPage(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney ApplyTerms(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney GoHomePage(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney UpdateLoanDuration(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        #region Builder

        public virtual IL0ConsumerJourney FillAndStop()
        {
            _submit = false;
            return this;
        }

        public virtual IL0ConsumerJourney WithDeclineDecision()
        {
            journey.Remove(typeof(ProcessingPage));
            journey.Remove(typeof(AcceptedPage));
            journey.Remove(typeof(DealDonePage));
            journey.Add(typeof(ProcessingPage), WaitForDeclinedPage);
            return this;
        }

        public virtual IL0ConsumerJourney WithAmount(int amount)
        {
            _amount = amount;
            return this;
        }

        public virtual IL0ConsumerJourney WithDuration(int duration)
        {
            _duration = duration;
            return this;
        }

        public virtual IL0ConsumerJourney WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public virtual IL0ConsumerJourney WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public virtual IL0ConsumerJourney WithMiddleName(string middleName)
        {
            _middleName = middleName;
            return this;
        }

        public virtual IL0ConsumerJourney WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public virtual IL0ConsumerJourney WithEmployerName(string employerName)
        {
            _employerName = employerName;
            return this;
        }

        public virtual IL0ConsumerJourney WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public virtual IL0ConsumerJourney WithMobilePhone(string mobilePhone)
        {
            _mobilePhone = mobilePhone;
            return this;
        }

        public virtual IL0ConsumerJourney WithGender(GenderEnum gender)
        {
            _gender = gender;
            return this;
        }

        public virtual IL0ConsumerJourney WithDateOfBirth(DateTime dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
            return this;
        }

        public virtual IL0ConsumerJourney WithNationalId(string nationalId)
        {
            throw new NotImplementedException(message: "Used only on Za");
        }

        public virtual IL0ConsumerJourney WithMotherMaidenName(string motherMaidenName)
        {
            throw new NotImplementedException(message: "Used only on Pl");
        }

        public virtual IL0ConsumerJourney WithPosteCode(string postCode)
        {
            _postCode = postCode;
            return this;
        }

        public virtual IL0ConsumerJourney WithAddresPeriod(string addresPeriod)
        {
            _addressPeriod = addresPeriod;
            return this;
        }

        public virtual IL0ConsumerJourney WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public virtual IL0ConsumerJourney WithAccountNumber(string accountNumber)
        {
            _accountNumber = accountNumber;
            return this;
        }

        public virtual IL0ConsumerJourney WithBankPeriod(string bankPeriod)
        {
            _bankPeriod = bankPeriod;
            return this;
        }

        public virtual IL0ConsumerJourney WithPin(string pin)
        {
            _pin = pin;
            return this;
        }

        public virtual IL0ConsumerJourney WithCardNumber(string cardNumber)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }
        public virtual IL0ConsumerJourney WithCardSecurity(string cardSecurity)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual IL0ConsumerJourney WithCardType(string cardType)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual IL0ConsumerJourney WithExpiryDate(string expiryDate)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual IL0ConsumerJourney WithStartDate(string startDate)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual IL0ConsumerJourney WithAutAdditionalDirrector()
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithEligibilityQuestions(bool activeCompany = true, bool director = true, bool guarantee = true, bool resident = true, bool debitCard = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithAdditionalDirectorName(string additionalDirectorName)
        {
            throw new NotImplementedException(message: "Used only on Wb"); throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithAdditionalDirectorSurName(string additionalDirectorSurName)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithAdditionalDirectorEmail(string additionalDirectorEmail)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessBankAccount(string businessBankAccount)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessBankPeriod(string businessBankPeriod)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessDebitCardNumber(string businessDebitCardNumber)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessDebitCardSecurity(string businessDebitCardSecurity)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessDebitCardType(string businessDebitCardType)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessDebitCardExpiryDate(string businessDebitExpiryDate)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual IL0ConsumerJourney WithBusinessDebitCardStartDate(string businessDebitStartDate)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }


        #endregion
    }
}
