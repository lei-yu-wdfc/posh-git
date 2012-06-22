using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    public abstract class BaseL0Journey
    {
        protected Dictionary<Type, Func<bool, BaseL0Journey>> journey = new Dictionary<Type, Func<bool, BaseL0Journey>>();

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

        protected String _bankName;
        protected String _bankAccountType;
        protected String _branchNumber;
        protected String _sortCode;
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
                if (CurrentPage.GetType() == pageType && pageType != typeof(HomePage))
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

        protected abstract BaseL0Journey ApplyForLoan(bool submit = true);

        protected abstract BaseL0Journey FillPersonalDetails(bool submit = true);

        protected abstract BaseL0Journey FillAddressDetails(bool submit = true);

        protected abstract BaseL0Journey FillAccountDetails(bool submit = true);

        protected abstract BaseL0Journey FillBankDetails(bool submit = true);

        protected virtual BaseL0Journey FillCardDetails(bool submit = true)
        {
            throw new NotImplementedException();
        }

        protected abstract BaseL0Journey WaitForAcceptedPage(bool submit = true);

        protected abstract BaseL0Journey WaitForDeclinedPage(bool submit = true);

        protected abstract BaseL0Journey FillAcceptedPage(bool submit = true);

        protected abstract BaseL0Journey GoToMySummaryPage(bool submit = true);

        public virtual BaseL0Journey IgnoreAcceptingLoanAndReturnToHomePageAndLogin(bool submit = true)
        {
            throw new NotImplementedException();
        }

        protected virtual BaseL0Journey AnswerEligibilityQuestions(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }
        protected virtual BaseL0Journey EnterBusinessDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }
        protected virtual BaseL0Journey DeclineAddAdditionalDirector(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseL0Journey AddAdditionalDirector(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseL0Journey EnterBusinessBankAccountDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseL0Journey EnterBusinessDebitCardDetails(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }
        protected virtual BaseL0Journey WaitForApplyTermsPage(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseL0Journey ApplyTerms(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseL0Journey GoHomePage(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey UpdateLoanDuration(bool submit = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        #region Builder

        public virtual BaseL0Journey FillAndStop()
        {
            _submit = false;
            return this;
        }

        public virtual BaseL0Journey WithDeclineDecision()
        {
            journey.Remove(typeof(ProcessingPage));
            journey.Remove(typeof(AcceptedPage));
            journey.Remove(typeof(DealDonePage));
            journey.Add(typeof(ProcessingPage), WaitForDeclinedPage);
            return this;
        }

        public virtual BaseL0Journey WithAmount(int amount)
        {
            _amount = amount;
            return this;
        }

        public virtual BaseL0Journey WithDuration(int duration)
        {
            _duration = duration;
            return this;
        }

        public virtual BaseL0Journey WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        public virtual BaseL0Journey WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        public virtual BaseL0Journey WithMiddleName(string middleName)
        {
            _middleName = middleName;
            return this;
        }

        public virtual BaseL0Journey WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public virtual BaseL0Journey WithEmployerName(string employerName)
        {
            _employerName = employerName;
            return this;
        }

        public virtual BaseL0Journey WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public virtual BaseL0Journey WithMobilePhone(string mobilePhone)
        {
            _mobilePhone = mobilePhone;
            return this;
        }

        public virtual BaseL0Journey WithGender(GenderEnum gender)
        {
            _gender = gender;
            return this;
        }

        public virtual BaseL0Journey WithDateOfBirth(DateTime dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
            return this;
        }

        public virtual BaseL0Journey WithNationalId(string nationalId)
        {
            throw new NotImplementedException(message: "Used only on Za");
        }

        public virtual BaseL0Journey WithMotherMaidenName(string motherMaidenName)
        {
            throw new NotImplementedException(message: "Used only on Pl");
        }

        public virtual BaseL0Journey WithPosteCode(string postCode)
        {
            _postCode = postCode;
            return this;
        }

        public virtual BaseL0Journey WithAddresPeriod(string addresPeriod)
        {
            _addressPeriod = addresPeriod;
            return this;
        }

        public virtual BaseL0Journey WithPassword(string password)
        {
            _password = password;
            return this;
        }

        public virtual BaseL0Journey WithBankName(string bankName)
        {
            _bankName = bankName;
            return this;
        }

        public virtual BaseL0Journey WithBankAccountType(string bankAccountType)
        {
            throw new NotImplementedException(message: "Used only on Za");
        }

        public virtual BaseL0Journey WithBranchNumber(string branchNumber)
        {
            throw new NotImplementedException(message: "Used only on Ca");
        }

        public virtual BaseL0Journey WithSortCode( string sortCode)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual BaseL0Journey WithAccountNumber(string accountNumber)
        {
            _accountNumber = accountNumber;
            return this;
        }

        public virtual BaseL0Journey WithBankPeriod(string bankPeriod)
        {
            _bankPeriod = bankPeriod;
            return this;
        }

        public virtual BaseL0Journey WithPin(string pin)
        {
            _pin = pin;
            return this;
        }

        public virtual BaseL0Journey WithCardNumber(string cardNumber)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }
        public virtual BaseL0Journey WithCardSecurity(string cardSecurity)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual BaseL0Journey WithCardType(string cardType)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual BaseL0Journey WithExpiryDate(string expiryDate)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual BaseL0Journey WithStartDate(string startDate)
        {
            throw new NotImplementedException(message: "Used only on Pl and Uk");
        }

        public virtual BaseL0Journey WithAdditionalDirrector()
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithEligibilityQuestions(bool activeCompany = true, bool director = true, bool guarantee = true, bool resident = true, bool debitCard = true)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithAdditionalDirectorName(string additionalDirectorName)
        {
            throw new NotImplementedException(message: "Used only on Wb"); throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithAdditionalDirectorSurName(string additionalDirectorSurName)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithAdditionalDirectorEmail(string additionalDirectorEmail)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessBankAccount(string businessBankAccount)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessBankPeriod(string businessBankPeriod)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessDebitCardNumber(string businessDebitCardNumber)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessDebitCardSecurity(string businessDebitCardSecurity)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessDebitCardType(string businessDebitCardType)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessDebitCardExpiryDate(string businessDebitExpiryDate)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        public virtual BaseL0Journey WithBusinessDebitCardStartDate(string businessDebitStartDate)
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }


        #endregion
    }
}
