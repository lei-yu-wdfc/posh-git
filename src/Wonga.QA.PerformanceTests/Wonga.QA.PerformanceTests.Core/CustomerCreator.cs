using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.PerformanceTests.Core
{
    public class CustomerCreator
    {
        public int NumberOfUsersToCreate = 1;
        private int _applicationId;

        #region Accounting
        //.Applications
        //ExternalId used across multiple tables to uniquely idetify the application
        private readonly String _externalId;
        //AccountId used across multiple tables to uniquely idetify the account
        private readonly String _accountId;
        private readonly int _productId;
        private readonly String _applicationDate;
        private readonly int _status;
        
        //SystemTransactions
        private readonly String _postedOn;
        private readonly int _scope; 
        private readonly int _type; 
        private readonly decimal _amount;
        private readonly decimal _transactionAmount;
        private readonly int _currency;
        private readonly String _createdOn;
        private readonly int _paymentStatus;

        #endregion

        #region Comms
        //Addresses
        private readonly String _postCode;
        
        //ContactPreferences
        private readonly int _acceptMarketingContact;
        
        //CustomerDetails
        private readonly String _forename;
        private readonly String _surname;
        private readonly String _homePhone;
        private readonly String _mobilePhone;
        private readonly String _workPhone;
        private readonly int _hasAccount;
        
        //CustomerReviewDetails
        private readonly int _requireReviewDetails;
        private readonly String _lastReviewedDetailsOn;
        
        //LegalDocuments
        private readonly int _documentType;
        
        //MobilePhoneVerification
        private readonly String _verificationId;
        private readonly String _pin;
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

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomerCreator()
        {
            #region Accounting

            //.Applications
            _externalId = GetNewGuid();
            _accountId = "'" + Get.GetId() + "'";
            _productId = 1;
            _applicationDate = GetCurrentDateTime();
            _status = 2;
            
            //SystemTransactions
            _postedOn = GetCurrentDateTime();
            _scope = 1;
            _type = 1;
            _amount = Get.GetLoanAmount();
            _transactionAmount = (decimal) 5.5;
            _currency = 826;
            _createdOn = GetCurrentDateTime();
            _paymentStatus = 1;
            #endregion

            #region Comms
            //Addresses
            _postCode = "'"+ Get.GetPostcode() + "'";
            
            //ContactPreferences
            _acceptMarketingContact = 0;
            
            //CustomerDetails
            _forename = "'"+ Get.GetName() + "'";
            _surname = "'"+ "Bulk User" + "'";
            _homePhone = Get.GetPhone();
            _mobilePhone = Get.GetPhone();
            _workPhone = Get.GetPhone();
            _hasAccount = 1;
            
            //CustomerReviewDetails
            _requireReviewDetails = 0;
            _lastReviewedDetailsOn = "'" + DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            
            //LegalDocuments
            _documentType = 1;
            
            //MobilePhoneVerification
            _verificationId = GetNewGuid();
            _pin = Get.GetVerificationPin();
            #endregion

            #region Ops
            _login ="'" + Get.GetEmail() + "'";
            #endregion

            #region Payments
            _isHardShip = 0;
            _remindBeforeEndLoan = 0;
            _canAddBankAccount = 1;

            //PaymentBankDetails
            _bankName = "'ABBEY'";
            _sortCode = "'161027'";
            _accountNumber = "'" + Get.GetBankAccountNumber("161027") + "'";
            _holderName = "'" + _forename.Replace(@"'", "") + " " + _surname.Replace(@"'", "") + "'";
            _accountOpenDate = GetCurrentDateTime();
            _countryCodeStr = "'" + Get.GetCountryCode() + "'";
            
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
            _dateOfBirth = "'"+ Get.GetDoB() + "'";
            _isDebtSale = 0;
            _confirmedFraud = 0;
            _isDispute = 0;
            _doNotRelend = 0;
            #endregion
        }

        /// <summary>
        /// Creates a customer
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
            accSysTransactInsert.Add("Amount", _transactionAmount);
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
            payFixInsert.Add("TransmissionFee", _transactionAmount);
            payFixInsert.Add("PromiseDate", _promiseDate);
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
            payTransInsert.Add("Amount", _transactionAmount);
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

        /// <summary>
        /// Insert Customer into Risk Database
        /// </summary>
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

        /// <summary>
        /// Retruns current datetime in yyyy-MM-dd HH:mm:ss format
        /// </summary>
        /// <returns></returns>
        public String GetCurrentDateTime()
        {
            return "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
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
