using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.PerformanceTests.Core
{
    public class CustomerCreator
    {
        public int NumberOfUsersToCreate = 100;
        private int _applicationId;

        #region Accounting
        //.Applications
        private readonly String _externalId; //D4AF1E77-EC86-417A-90BD-A8E02BC6E65C
        private readonly String _accountId;//F0C16F91-74F6-4460-B721-916126A9F8F9
        private readonly int _productId;//1
        private readonly String _applicationDate;
        private readonly int _status;//2
        
        //SystemTransactions
        private readonly String _postedOn; //2012-08-24 14:28:11.000
        private readonly int _scope; //1
        private readonly int _type; //1
        private readonly float _amount; //100.0
        private readonly int _currency; //826
        private readonly String _createdOn;
        private readonly int _paymentStatus; //1

        #endregion

        #region Comms
        //Addresses
        private readonly String _postCode;
        
        //ContactPreferences
        private readonly int _acceptMarketingContact;//0
        
        //CustomerDetails
        private readonly String _forename;
        private readonly String _surname;
        private readonly String _homePhone;
        private readonly String _mobilePhone;
        private readonly String _workPhone;
        private readonly int _hasAccount;//1
        
        //CustomerReviewDetails
        private readonly int _requireReviewDetails;//0
        private readonly String _lastReviewedDetailsOn;
        
        //LegalDocuments
        private readonly String _commsApplicationId;
        private readonly int _documentType;//1
        
        //MobilePhoneVerification
        private readonly String _verificationId; //5756E273-3581-488C-A343-D7333743DBF7
        private readonly String _pin; //6166
        #endregion

        #region Ops
        private readonly String _login;
        #endregion

        #region Payments
        private readonly int _isHardShip;
        private readonly int _remindBeforeEndLoan;
        private readonly int _canAddBankAccount;

        private readonly String _bankName;
        private readonly String _sortCode;
        private readonly String _accountNumber;
        private readonly String _holderName;
        private readonly String _accountOpenDate;
        private readonly String _countryCodeStr;

        private readonly String _cardType;
        private readonly String _maskedNumber;
        private readonly String _expiryDate;

        private readonly String _paymentCardId;
        private readonly String _bankAccountId;
        #endregion

        #region OpsSagas

        private readonly int _termsAgreed;
        private readonly int _applicationAccepted;

        private readonly int _inRepaymentArrangement;
        private readonly int _inDca;

        private readonly String _promiseDate;
        #endregion

        #region Risk
        private readonly int _isCanceled;
        private readonly int _isCounterOffer;
        private readonly int _creditLimit;
        private readonly int _termLimit;
        private readonly int _wasExtended;
        private readonly int _wasTopUp;
        private readonly int _usedManualVerification;
        private readonly int _accountRank;
        private readonly String _dateOfBirth;
        private readonly int _isDebtSale;
        private readonly int _confirmedFraud;
        private readonly int _isDispute;
        private readonly int _doNotRelend;
        #endregion

        public CustomerCreator()
        {
            #region Accounting

            var cutomer = CustomerBuilder.New().WithPassword("Passw0rd");
            //.Applications
            _externalId = GetNewGuid();
            _accountId = "'" + cutomer.Id.ToString() + "'";
            _productId = 1;
            _applicationDate = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"'" ;
            _status = 2;
            
            //SystemTransactions
            _postedOn = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            _scope = 1;
            _type = 1;
            _amount = 100.0f;
            _currency = 826;
            _createdOn = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            _paymentStatus = 1;
            #endregion

            #region Comms
            //Addresses
            _postCode = "'"+ "AA1 1AA" + "'";
            
            //ContactPreferences
            _acceptMarketingContact = 0;
            
            //CustomerDetails
            _forename = "'"+ cutomer.Forename + "'";
            _surname = "'"+ cutomer.Surname + "'";
            _homePhone = "02070707007";
            _mobilePhone = "07070707007";
            _workPhone = "02030707007";
            _hasAccount = 1;
            
            //CustomerReviewDetails
            _requireReviewDetails = 0;
            _lastReviewedDetailsOn = "'" + DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            
            //LegalDocuments
            _commsApplicationId = _accountId;
            _documentType = 1;
            
            //MobilePhoneVerification
            _verificationId = GetNewGuid();
            _pin = "6166";
            #endregion

            #region Ops
            _login ="'" + cutomer.Email + "'";
            #endregion

            #region Payments
            _isHardShip = 0;
            _remindBeforeEndLoan = 0;
            _canAddBankAccount = 1;

            //PaymentBankDetails
            _bankName = "'ABBEY'";
            _sortCode = "'938600'";
            _accountNumber = "'42368003'";
            _holderName = "'Forename Surname '";
            _accountOpenDate = "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            _countryCodeStr = "'UK'";

            //PaymentCardDetails
            _cardType = "'visa'";
            _maskedNumber = "'**** **** **** 1111'";
            _expiryDate = "'" + DateTime.Now.AddYears(2).ToString("yyyy-MM-dd") + "'";

            _paymentCardId = GetNewGuid();
            _bankAccountId = GetNewGuid();
            #endregion

            #region OpsSagas

            _termsAgreed = 1;
            _applicationAccepted = 1;
            _inDca = 0;
            _inRepaymentArrangement = 0;

            const int term = 10;
            _promiseDate = "'" + DateTime.Now.AddDays(term).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            #endregion

            #region Risk
            _isCanceled = 0;
            _isCounterOffer = 0;
            _creditLimit = 1000;
            _termLimit = 30;
            _wasExtended = 0;
            _wasTopUp = 0;
            _usedManualVerification = 0;

            _accountRank = 0;
            _dateOfBirth = "'1980-01-01 00:00:00.000'";
            _isDebtSale = 0;
            _confirmedFraud = 0;
            _isDispute = 0;
            _doNotRelend = 0;
            #endregion
        }

        /// <summary>
        /// Creates the customer
        /// </summary>
        public void CreateCustomer()
        {
            var cust = new CustomerCreator();
            //Execute SQL Queries
            cust.InsertAccoutingDb();
            cust.InsertCommsDb();
            cust.InsertOpsDb();
            cust.InsertPaymentsDb();
            cust.InsertOpsSagasDb();
            cust.InsertRiskDb();
        }

        /// <summary>
        /// Insert Customer into Accounting Database
        /// </summary>
        public void InsertAccoutingDb()
        {
            var operations = new DbOperations();

            var accAppInsert = new Insert("Accounting", "accounting.Applications");
            accAppInsert.Add("ExternalId", _externalId);
            accAppInsert.Add("AccountId", _accountId);
            accAppInsert.Add("ProductId", _productId);
            accAppInsert.Add("ApplicationDate", _applicationDate);
            accAppInsert.Add("Status", _status);
            operations.Insert(accAppInsert);
            
            var accSysTransactInsert = new Insert("Accounting", "accounting.SystemTransactions");
            accSysTransactInsert.Add("TransactionId", GetNewGuid());
            accSysTransactInsert.Add("ApplicationId", _externalId);
            accSysTransactInsert.Add("AccountId", _accountId);
            accSysTransactInsert.Add("PostedOn", _postedOn);
            accSysTransactInsert.Add("Scope", _scope);
            accSysTransactInsert.Add("Type", _type);
            accSysTransactInsert.Add("Amount", _amount);
            accSysTransactInsert.Add("Currency", _currency);
            accSysTransactInsert.Add("CreatedOn", _createdOn);
            accSysTransactInsert.Add("PaymentStatus", _paymentStatus);
            operations.Insert(accSysTransactInsert);

            accSysTransactInsert.Remove("TransactionId");
            accSysTransactInsert.Remove("Type");
            accSysTransactInsert.Remove("Amount");
            accSysTransactInsert.Remove("PaymentStatus");
            accSysTransactInsert.Add("TransactionId", GetNewGuid());
            accSysTransactInsert.Add("Type", 2);
            accSysTransactInsert.Add("Amount", _amount * 5.5 / 100);
            accSysTransactInsert.Add("PaymentStatus", 0);
            operations.Insert(accSysTransactInsert);

            _applicationId = operations.GetMaxId("Accounting", "Select max(ApplicationId) from accounting.Applications");
        }

        /// <summary>
        /// Insert Customer in to Comms Database
        /// </summary>
        public void InsertCommsDb()
        {
            var operations = new DbOperations();

            var commsAddrInsert = new Insert("Comms", "comms.Addresses");
            commsAddrInsert.Add("ExternalId", GetNewGuid());
            commsAddrInsert.Add("AccountId", _accountId);
            commsAddrInsert.Add("PostCode", _postCode);
            operations.Insert(commsAddrInsert);

            var commsCntInsert = new Insert("Comms", "comms.ContactPreferences");
            commsCntInsert.Add("AccountId", _accountId);
            commsCntInsert.Add("AcceptMarketingContact", _acceptMarketingContact);
            operations.Insert(commsCntInsert);

            var commsCusDetInsert = new Insert("Comms", "comms.CustomerDetails");
            commsCusDetInsert.Add("AccountId", _accountId);
            commsCusDetInsert.Add("DateOfBirth", _dateOfBirth);
            commsCusDetInsert.Add("Gender", 0);
            commsCusDetInsert.Add("Title", 3);
            commsCusDetInsert.Add("Forename", _forename);
            commsCusDetInsert.Add("Surname", _surname);
            commsCusDetInsert.Add("MiddleName", "'MiddleName'");
            commsCusDetInsert.Add("HomePhone", _homePhone);
            commsCusDetInsert.Add("MobilePhone", _mobilePhone);
            commsCusDetInsert.Add("WorkPhone", _workPhone);
            commsCusDetInsert.Add("Email", _login);
            commsCusDetInsert.Add("CreatedOn", _createdOn);
            commsCusDetInsert.Add("HasAccount", _hasAccount);
            operations.Insert(commsCusDetInsert);

            var commsCusRevInsert = new Insert("Comms", "comms.CustomerReviewDetails");
            commsCusRevInsert.Add("AccountId", _accountId);
            commsCusRevInsert.Add("RequireReviewDetails", _requireReviewDetails);
            commsCusRevInsert.Add("LastReviewedDetailsOn", _lastReviewedDetailsOn);
            commsCusRevInsert.Add("CreatedOn", _createdOn);
            operations.Insert(commsCusRevInsert);

            var commsLegDocInsert = new Insert("Comms", "comms.LegalDocuments");
            commsLegDocInsert.Add("ExternalId", GetNewGuid());
            commsLegDocInsert.Add("AccountId", _accountId);
            commsLegDocInsert.Add("ApplicationId", _externalId);
            commsLegDocInsert.Add("DocumentType", _documentType);
            operations.Insert(commsLegDocInsert);

            var commsMobVerInsert = new Insert("Comms", "comms.MobilePhoneVerification");
            commsMobVerInsert.Add("VerificationId", _verificationId);
            commsMobVerInsert.Add("AccountId", _accountId);
            commsMobVerInsert.Add("MobilePhone", _mobilePhone);
            commsMobVerInsert.Add("Pin", _pin);
            operations.Insert(commsMobVerInsert);
        }

        /// <summary>
        /// Insert Customer into Ops Database
        /// </summary>
        public void InsertOpsDb()
        {
            var operations = new DbOperations();

            var opsAccInsert = new Insert("Ops", "ops.Accounts");
            opsAccInsert.Add("ExternalId", _accountId);
            opsAccInsert.Add("Login", _login);
            opsAccInsert.Add("CreatedOn", _createdOn);
            opsAccInsert.Add("Password", "CONVERT(varbinary(64), '" + Get.GetPassword() + "')");
            opsAccInsert.Add("Salt", "CONVERT(varbinary(8), '" + Get.GetPassword() + "')");
            operations.Insert(opsAccInsert);
        }

        /// <summary>
        /// Insert Customer into Payments Database
        /// </summary>
        public void InsertPaymentsDb()
        {
            var operations = new DbOperations();
            
            var payAccInsert = new Insert("Payments", "payment.AccountPreferences");
            payAccInsert.Add("AccountId", _accountId);
            payAccInsert.Add("IsHardShip", _isHardShip);
            payAccInsert.Add("CreatedOn", _createdOn);
            payAccInsert.Add("RemindBeforeEndLoan", _remindBeforeEndLoan);
            payAccInsert.Add("CanAddBankAccount", _canAddBankAccount);
            operations.Insert(payAccInsert);

            var payAppInsert = new Insert("Payments", "payment.Applications");
            payAppInsert.Add("ExternalId", _externalId);
            payAppInsert.Add("AccountId", _accountId);
            payAppInsert.Add("ProductId", _productId);
            payAppInsert.Add("Currency", _currency);
            payAppInsert.Add("BankAccountGuid", _bankAccountId);
            payAppInsert.Add("PaymentCardGuid", _paymentCardId);
            payAppInsert.Add("ApplicationDate", _applicationDate);
            payAppInsert.Add("CreatedOn", _createdOn);
            operations.Insert(payAppInsert);

            var payFixInsert = new Insert("Payments", "payment.FixedTermLoanApplications");
            payFixInsert.Add("ApplicationId", _applicationId);
            payFixInsert.Add("LoanAmount", _amount);
            payFixInsert.Add("MonthlyInterestRate", 30);
            payFixInsert.Add("TransmissionFee", 5.50);
            payFixInsert.Add("PromiseDate", "'" + DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss") +"'");
            operations.Insert(payFixInsert);

            var payBankBInsert = new Insert("Payments", "payment.BankAccountsBase");
            payBankBInsert.Add("ExternalId", _bankAccountId);
            payBankBInsert.Add("BankName", _bankName);
            payBankBInsert.Add("BankCode", _sortCode);
            payBankBInsert.Add("AccountNumber", _accountNumber);
            payBankBInsert.Add("HolderName", _holderName);
            payBankBInsert.Add("AccountOpenDate", _accountOpenDate);
            payBankBInsert.Add("CountryCode", _countryCodeStr);
            payBankBInsert.Add("CreatedOn", _createdOn);
            operations.Insert(payBankBInsert);

            var bankId = operations.GetMaxId("Payments", "Select max(BankAccountId) from payment.BankAccountsBase");
            var payPerBanInsert = new Insert("Payments", "payment.PersonalBankAccounts");
            payPerBanInsert.Add("BankAccountId", bankId);
            payPerBanInsert.Add("AccountId", _accountId);
            operations.Insert(payPerBanInsert);

            var payCardBInsert = new Insert("Payments", "payment.PaymentCardsBase");
            payCardBInsert.Add("ExternalId", _paymentCardId);
            payCardBInsert.Add("Type", _cardType);
            payCardBInsert.Add("MaskedNumber", _maskedNumber);
            payCardBInsert.Add("ExpiryDate", _expiryDate);
            payCardBInsert.Add("CreatedOn", _createdOn);
            operations.Insert(payCardBInsert);

            var cardId = operations.GetMaxId("Payments", "Select max(PaymentCardId) from payment.PaymentCardsBase");
            var payCardsInsert = new Insert("Payments", "payment.PersonalPaymentCards");
            payCardsInsert.Add("PaymentCardId", cardId);
            payCardsInsert.Add("AccountId", _accountId);
            operations.Insert(payCardsInsert);

            var payTransInsert = new Insert("Payments", "payment.Transactions");
            payTransInsert.Add("ExternalId", GetNewGuid());
            payTransInsert.Add("ApplicationId", _applicationId);
            payTransInsert.Add("PostedOn", _createdOn);
            payTransInsert.Add("Scope", _scope);
            payTransInsert.Add("Type", "'CashAdvance'");
            payTransInsert.Add("Reference", "'Fixed term loan initial advance'");
            payTransInsert.Add("Amount", _amount);
            payTransInsert.Add("Currency", _currency);
            payTransInsert.Add("CreatedOn", _createdOn);
            operations.Insert(payTransInsert);

            payTransInsert.Remove("ExternalId");
            payTransInsert.Add("ExternalId", GetNewGuid());
            payTransInsert.Remove("Type");
            payTransInsert.Add("Type", "'Fee'");
            payTransInsert.Remove("Reference");
            payTransInsert.Add("Reference", "'Fixed term loan transamission fee'");
            payTransInsert.Remove("Amount");
            payTransInsert.Add("Amount", 5.50);
            operations.Insert(payTransInsert);
        }

        /// <summary>
        /// Insert Customer into OpsSagas Database
        /// </summary>
        public void InsertOpsSagasDb()
        {
            var operations = new DbOperations();
            var uniqueId = GetNewGuid();

            var opSaFixInsert = new Insert("OpsSagas", "dbo.FixedTermLoanSagaEntity");
            opSaFixInsert.Add("Id", uniqueId);
            opSaFixInsert.Add("ApplicationId", _applicationId);
            opSaFixInsert.Add("ApplicationGuid", _externalId);
            opSaFixInsert.Add("TermsAgreed", _termsAgreed);
            opSaFixInsert.Add("ApplicationAccepted", _applicationAccepted);
            opSaFixInsert.Add("AccountGuid", _accountId);
            operations.Insert(opSaFixInsert);

            var opSaLDueInsert = new Insert("OpsSagas", "dbo.LoanDueDateNotificationSagaEntity");
            opSaLDueInsert.Add("Id", uniqueId);
            opSaLDueInsert.Add("ApplicationId", _externalId);
            opSaLDueInsert.Add("AccountId", _accountId);
            opSaLDueInsert.Add("TermsAgreed", _termsAgreed);
            opSaLDueInsert.Add("ApplicationAccepted", _applicationAccepted);
            operations.Insert(opSaLDueInsert);

            var opSaAppInsert = new Insert("OpsSagas", "dbo.ApplicationSagaEntity");
            opSaAppInsert.Add("Id", uniqueId);
            opSaAppInsert.Add("ApplicationId", _applicationId);
            opSaAppInsert.Add("ApplicationGuid", _externalId);
            opSaAppInsert.Add("InRepaymentArrangement", _inRepaymentArrangement);
            opSaAppInsert.Add("InDca", _inDca);
            operations.Insert(opSaAppInsert);

            var opSaEmailInsert = new Insert("OpsSagas", "dbo.EmailLoanAgreementEntity");
            opSaEmailInsert.Add("Id", uniqueId);
            opSaEmailInsert.Add("AccountId", _accountId);
            opSaEmailInsert.Add("ApplicationId", _externalId);
            opSaEmailInsert.Add("FileId", GetNewGuid());
            opSaEmailInsert.Add("TermsAgreed", _termsAgreed);
            operations.Insert(opSaEmailInsert);
        }

        public void InsertRiskDb()
        {
            var operations = new DbOperations();

            var riskAppInsert = new Insert("Risk", "risk.RiskApplications");
            riskAppInsert.Add("ApplicationId", _externalId);
            riskAppInsert.Add("AccountId", _accountId);
            riskAppInsert.Add("Currency", _currency);
            riskAppInsert.Add("PromiseDate", _promiseDate);
            riskAppInsert.Add("ApplicationDate", _createdOn);
            riskAppInsert.Add("LoanAmount", _amount);
            riskAppInsert.Add("PaymentCardId", _paymentCardId);
            riskAppInsert.Add("BankAccountId", _bankAccountId);
            riskAppInsert.Add("IsCanceled", _isCanceled);
            riskAppInsert.Add("OriginalLoanAmount", _amount);
            riskAppInsert.Add("IsCounterOffer", _isCounterOffer);
            riskAppInsert.Add("CreditLimit", _creditLimit);
            riskAppInsert.Add("TermLimit", _termLimit);
            riskAppInsert.Add("WasExtended", _wasExtended);
            riskAppInsert.Add("WasTopUp", _wasTopUp);
            riskAppInsert.Add("UsedManualVerification", _usedManualVerification);
            operations.Insert(riskAppInsert);

            var riskAccInsert = new Insert("Risk", "risk.RiskAccounts");
            riskAccInsert.Add("AccountId", _externalId);
            riskAccInsert.Add("AccountRank", _accountRank);
            riskAccInsert.Add("DateOfBirth", _dateOfBirth);
            riskAccInsert.Add("PostCode", _postCode);
            riskAccInsert.Add("IsDebtSale", _isDebtSale);
            riskAccInsert.Add("CreditLimit", _creditLimit);
            riskAccInsert.Add("ConfirmedFraud", _confirmedFraud);
            riskAccInsert.Add("Surname", _surname);
            riskAccInsert.Add("IsHardShip", _isHardShip);
            riskAccInsert.Add("IsDispute", _isDispute);
            riskAccInsert.Add("DoNotRelend", _doNotRelend);
            riskAccInsert.Add("Forename", _forename);
            riskAccInsert.Add("HasAccount", _hasAccount);
            operations.Insert(riskAccInsert);
        }

        /// <summary>
        /// Returns unique id sorrouned by '
        /// </summary>
        /// <returns></returns>
        public String GetNewGuid()
        {
            return "'" + Guid.NewGuid().ToString() + "'";
        }

        [Test]
        public void InsertUsers()
        {
            for (int i = 0; i < NumberOfUsersToCreate; i++)
            {
                CreateCustomer();
            }
        }
    }
}
