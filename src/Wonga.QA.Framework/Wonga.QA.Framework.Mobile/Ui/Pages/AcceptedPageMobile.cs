﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Ui;

namespace Wonga.QA.Framework.Mobile.Ui.Pages
{
    public class AcceptedPageMobile : BasePageMobile, IDecisionPage
    {
        private readonly IWebElement _form;
        //private readonly IWebElement _nodeWrapper;
        private readonly IWebElement _totalToRepay;
        private readonly IWebElement _repaymentDate;
        private readonly IWebElement _acceptBusinessLoanLink;
        private readonly IWebElement _acceptGuarantorLoanLink;
        private IWebElement _agreementConfirm;
        private IWebElement _directDebitConfirm;
        private IWebElement _initials;
        private IWebElement _initials2;
        private IWebElement _initials3;
        private IWebElement _signature;
        private IWebElement _dateOfAgreement;
        private IWebElement _continueTermsButton;
        private IWebElement _continueDirectDebitButton;
        private readonly IWebElement _detailsTable;
        private readonly IWebElement _loanAmount;
        private readonly IWebElement _termsOfLoan;
        private readonly IWebElement _totalToPayOnPaymentDate;
        private readonly IWebElement _principalAmountBorrowed;
        private readonly IWebElement _principalAmountToBeTransfered;
        private readonly IWebElement _totalCostOfCredit;
        private readonly IWebElement _totalAmountDueUnderTheAgreement;
        private readonly IWebElement _paymentDueDate;

        public String Initials1 { set{_initials.SendValue(value);} }
        public String Initials2 { set { _initials2.SendValue(value); } }
        public String Initials3 { set { _initials3.SendValue(value); } }
        public String Signature { set{_signature.SendValue(value);} }
        public String DateOfAgreement { set{_signature.SendValue(value);} }

        public AcceptedPageMobile(MobileUiClient client)
            : base(client)
        {            
             switch(Config.AUT)
            {
                case (AUT.Wb):
                    _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.FormId));
                    _acceptBusinessLoanLink = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.AcceptBusinessLoan));
                    _acceptGuarantorLoanLink = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.AcceptGuarantorLoan));
                    _loanAmount = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.LoanAmount));
                    _termsOfLoan = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.TermsOfLoan));
                    break;
                case(AUT.Za):
                    _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.FormId));
                    _totalToRepay = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.TotalToRepay));
                    _repaymentDate = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.RepaymentDate));

                    _detailsTable = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DetailsTable));
                    _paymentDueDate = _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.PaymentDueDate));
                    _loanAmount = _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.LoanAmount));
                    _totalToPayOnPaymentDate = _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.TotalToPayOnPaymentDate));

                    break;
                case (AUT.Ca):
                    _form = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.FormId));
                    _totalToRepay = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.TotalToRepay));
                    _repaymentDate = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.RepaymentDate));
                    _detailsTable = Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DetailsTable));
                    _principalAmountBorrowed =
                        _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.PrincipalAmountBorrowed));
                    _principalAmountToBeTransfered =
                        _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.PrincipalAmountToBeTransfered));
                    _totalCostOfCredit = _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.TotalCostOfCredit));
                    _totalAmountDueUnderTheAgreement =
                        _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.TotalAmountDueUnderTheAgreement));
                    _paymentDueDate = _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.PaymentDueDate));
                    _loanAmount = _detailsTable.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.LoanAmount));
                    break;
                case (AUT.Uk):
                    _form = Content.FindEitherElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.FormId), By.CssSelector("#wonga-loan-approve-form"));

                    //_nodeWrapper = Content.FindElement(By.CssSelector(UiMap.Get.AcceptedPage.NodeWrap));
                    break;
                 default:
                     throw new NotImplementedException();
            }
        }

        public String GetTotalToRepay
        {
            get { return _totalToRepay.Text; }
        }
        public String GetRepaymentDate
        {
            get { return _repaymentDate.Text; }
        }
        public String GetPrincipalAmountBorrowed
        {
            get { return _principalAmountBorrowed.Text; }
        }
        public String GetPrincipalAmountToBeTransfered
        {
            get { return _principalAmountToBeTransfered.Text; }
        }
        public String GetTotalCostOfCredit
        {
            get { return _totalCostOfCredit.Text; }
        }
        public String GetTotalAmountDueUnderTheAgreement
        {
            get { return _totalAmountDueUnderTheAgreement.Text; }
        }
        public String GetPaymentDueDate
        {
            get { return _paymentDueDate.Text; }
        }
        public String GetLoanAmount
        {
            get { return _loanAmount.Text.Replace(" ", "").Replace("*", ""); }
        }
        public String GetTermsOfLoan
        {
            get { return _termsOfLoan.Text; }
        }
        public String GetTotalToPayOnPaymentDate
        {
            get { return _totalToPayOnPaymentDate.Text.Replace(" ", "").Replace("*", ""); }
        }
        public String GetNameInLoanAgreement
        {
            get { return Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.NameInLoanAgreement)).Text; }
        }
        public String GetNameInDirectDebit
        {
            get { return Content.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.NameInDirectDebit)).Text.Replace("Full name(s) Payee/Account Holder: ", ""); }  
        }
        public void SignAgreementConfirm()
        {
            _agreementConfirm = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.AgreementConfirm));
            _agreementConfirm.Click();
        }

        public void SignDirectDebitConfirm()
        {
            _directDebitConfirm = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DirectDebitConfirm));
            _directDebitConfirm.Click();
        }

        public void SignTermsMainApplicant()
        {
            _acceptBusinessLoanLink.Click();
        }

        public void SignTermsGuarantor()
        {
            _acceptGuarantorLoanLink.Click();
        }

        public void SignConfirmCaL0(string date, string firstName, string lastName)
        {
            SignLoanAgreementL0(date, firstName, lastName);
            SignLoanTerms();
            SignDirectDebit();
        }

        public void SignConfirmCaLn(string date, string firstName, string lastName)
        {
            SignLoanAgreementLn(date, firstName, lastName);
            SignLoanTerms();
            SignDirectDebit();
        }

        public void SignLoanAgreementL0(string date,string firstName, string lastName)
        {
            _initials = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.Initials1));
            _initials2 = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.Initials2));
            _initials3 = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.Initials3));
            _signature = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.Signature));
            _dateOfAgreement = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DateOfAgreement));
            _continueTermsButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.ContinueTermsButton));
           
            string initials = string.Format("{0}{1}", firstName[0], lastName[0]);
            string signature = string.Format("{0} {1}", firstName, lastName);
            _initials.SendKeys(initials);
            _initials2.SendKeys(initials);
            _initials3.SendKeys(initials);
            _signature.SendKeys(signature);
            _dateOfAgreement.SendKeys(date);
            _signature.Click();
            _continueTermsButton.Click();
        }
        public void SignLoanAgreementLn(string date,string firstName, string lastName)
        {
            _signature = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.Signature));
            _dateOfAgreement = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DateOfAgreement));
            _continueTermsButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.ContinueTermsButton));

            string signature = string.Format("{0} {1}", firstName, lastName);
            _signature.SendKeys(signature);
            _dateOfAgreement.SendKeys(date);
            _signature.Click();
            _continueTermsButton.Click();
        }
        public void SignLoanTerms()
        {
            _agreementConfirm = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.AgreementConfirm));
            _continueDirectDebitButton = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.ContinueDirectDebitButton));
            _agreementConfirm.Click();
            _continueDirectDebitButton.Click();
        }
        public void SignDirectDebit()
        {
            _directDebitConfirm = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DirectDebitConfirm));
            _directDebitConfirm.Click();
        }
        
        public void SignConfirmZA()
        {
            _agreementConfirm = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.AgreementConfirm));
            _directDebitConfirm = _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.DirectDebitConfirm));
            _agreementConfirm.Click();
            _directDebitConfirm.Click();
        }


        public IApplyPage Submit()
        {
            _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.SubmitButton)).Click();
            //if (Config.AUT.Equals(AUT.Wb))
            //    return new ReferPage(Client);
            return Do.With.Timeout(2).Until(() => new DealDonePage(Client));
        }

        public bool IsAgreementFormDisplayed()
        {
            return _form.FindElement(By.CssSelector(UiMapMobile.Get.AcceptedPage.AgreementTitle)).Displayed;
        }
    }
}
