using System;
using System.ServiceModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.ThirdParties.SalesforceApi;
using System.Collections.Generic;


namespace Wonga.QA.Framework.ThirdParties
{
    public class Salesforce
    {
        public enum ApplicationStatus
        {
            New = 0,
            Referral = 1,
            UserPending = 2,
            Deferral = 3,
            Accepted = 4,
            UserAccepted = 5,
            TermsAgreed = 6,
            UserDecline = 7,
            Cancelled = 8,
            Live = 9,
            DueToday = 10,
            InArrears = 11,
            Fraud = 12,
            DCA = 13,
            DMPRepaymentArrangement = 14,
            DMPRepaymentArrangementBroken = 15,
            Hardship = 16,
            Bankrupt = 17,
            RepaymentArrangement = 18,
            RepaymentArrangementBroken = 19,
            Complaint = 20,
            ManagementReview = 21,
            Refund = 22,
            ClearMyBalance = 23,
            DebtSurveillance = 24,
            DebtSold = 25,
            PaidInFull = 26,
            SettledInFull = 27,
            WrittenOff = 28,
        }

        public string SalesforceUsername { get; set; }
        public string SalesforcePassword { get; set; }
        public string SalesforceUrl { get; set; }

        public Salesforce()
        {
            // default
            SalesforceUsername = Config.SalesforceApi.Username;
            SalesforcePassword = Config.SalesforceApi.Password;
            SalesforceUrl = Config.SalesforceApi.Home.ToString();
        }

        /// <summary>
        /// Constructs binding instance to be used with salesforce soap client.
        /// We do this imperatively to avoid complex .config file management in Tests.
        /// </summary>
        /// <returns></returns>
        private Binding ConstructBinding()
        {
            const double closeTimeout = 1.0;
            const double openTimeout = 1.0;
            const double receiveTimeout = 10.0;
            const double sendTimeout = 1.0;
            const int maxBufferSize = 65536;
            const int maxBufferPoolSize = 524288;
            const int maxReceivedMessageSize = 65536;
            const int readerQuotasMaxDepth = 32;

            var result = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

            result.CloseTimeout = TimeSpan.FromMinutes(closeTimeout);
            result.OpenTimeout = TimeSpan.FromMinutes(openTimeout);
            result.ReceiveTimeout = TimeSpan.FromMinutes(receiveTimeout);
            result.SendTimeout = TimeSpan.FromMinutes(sendTimeout);
            result.AllowCookies = false;
            result.BypassProxyOnLocal = false;
            result.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            result.MaxBufferSize = maxBufferSize;
            result.MaxBufferPoolSize = maxBufferPoolSize;
            result.MaxReceivedMessageSize = maxReceivedMessageSize;
            result.MessageEncoding = WSMessageEncoding.Text;
            result.TextEncoding = Encoding.UTF8;
            result.TransferMode = TransferMode.Buffered;
            result.UseDefaultWebProxy = true;
            result.ReaderQuotas.MaxDepth = readerQuotasMaxDepth;
            result.ReaderQuotas.MaxStringContentLength = 8192;
            result.ReaderQuotas.MaxArrayLength = 16384;
            result.ReaderQuotas.MaxBytesPerRead = 4096;
            result.ReaderQuotas.MaxNameTableCharCount = 261384;
            result.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            result.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            result.Security.Transport.Realm = "";
            result.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            result.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;

            return result;
        }
        private SoapClient Login(out string sessionId)
        {
            SessionHeader sessionHeader;
            var result = Login(out sessionHeader);
            sessionId = sessionHeader.sessionId;
            return result;
        }

        private SoapClient Login(out SessionHeader sessionHeader)
        {
            sessionHeader = null;
            Binding binding = ConstructBinding();
            var client = new SoapClient(binding, new EndpointAddress(SalesforceUrl));
            LoginResult loginResult;
            try
            {
                string username = SalesforceUsername;
                string password = SalesforcePassword;
                loginResult = client.login(null, username, password);
            }
            catch (Exception e)
            {
                throw new Exception("Unexpected error logging-in to Salesforce", e);
            }

            if (loginResult.passwordExpired)
            {
                throw new Exception("Salesforce password was expired");
            }

            client = new SoapClient(client.Endpoint.Binding, new EndpointAddress(loginResult.serverUrl));
            sessionHeader = new SessionHeader {sessionId = loginResult.sessionId};
            return client;
        }

        public bool ApplicationExists(Guid applicationId)
        {
			return GetApplicationById(applicationId) != null;
        }

        public Loan_Application__c GetApplicationById(Guid applicationId)
        {
            string sessionId;
            SoapClient client = Login(out sessionId);
            SessionHeader sessionHeader = new SessionHeader {sessionId = sessionId};

            var query =
                String.Format("Select l.V3_Application_Id__c, l.Transmission_Fee__c, l.Status__c, l.CCIN__c, l.Service_Fee__c, " +
                              "l.Promise_Date__c, l.Number_Of_Weeks__c, l.Next_Due_Date__c, l.Monthly_Interest_Rate__c, " +
                              "l.Loan_Amount__c, l.Initiation_Fee__c, l.Customer_Account__c, l.CurrencyIsoCode, l.Application_Fee__c, " +
							  "l.SignedOn__c, l.Customer_Account__r.V3_Organization_Id__c, " +
                              "l.Application_Date__c, l.Status_ID__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}'",
                              applicationId);

            var result = client.query(sessionHeader, null, null, null, query);

			if (result == null || result.records == null) throw new Exception(string.Format("Unable to retrieve loan application by id={0}", applicationId));

            return result.records.FirstOrDefault() as Loan_Application__c;
        }

		public Loan_Application__c GetApplicationWithOrganisationById(Guid applicationId, Guid organisationId)
		{
			string sessionId;
			SoapClient client = Login(out sessionId);
			SessionHeader sessionHeader = new SessionHeader { sessionId = sessionId };

			var query =
				String.Format("Select l.V3_Application_Id__c, l.Transmission_Fee__c, l.Status__c, l.CCIN__c, l.Service_Fee__c, " +
							  "l.Promise_Date__c, l.Number_Of_Weeks__c, l.Next_Due_Date__c, l.Monthly_Interest_Rate__c, " +
							  "l.Loan_Amount__c, l.Initiation_Fee__c, l.Customer_Account__c, l.CurrencyIsoCode, l.Application_Fee__c, " +
							  "l.SignedOn__c, l.Customer_Account__r.V3_Organization_Id__c, " +
                              "l.Application_Date__c, l.Status_ID__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.Customer_Account__r.V3_Organization_Id__c = '{1}'",
							  applicationId, organisationId);

			var result = client.query(sessionHeader, null, null, null, query);

			if (result == null || result.records == null) throw new Exception(string.Format("Unable to retrieve loan application by id={0}", applicationId));

			return result.records.FirstOrDefault() as Loan_Application__c;
		}

		public Loan_Application__c GetApplicationByCustomQuery(Guid applicationId, string query)
		{
			string sessionId;
			var client = Login(out sessionId);
			var sessionHeader = new SessionHeader { sessionId = sessionId };
	
			var result = client.query(sessionHeader, null, null, null, query);

			if (result == null || result.records == null) throw new Exception(string.Format("Unable to retrieve loan application by id={0}", applicationId));

			return result.records.FirstOrDefault() as Loan_Application__c;
		}

		public Contact GetContactByStatus(Guid contactId, int status)
		{
			string sessionId;
			var client = Login(out sessionId);
			var sessionHeader = new SessionHeader { sessionId = sessionId };

			var query =
				String.Format("Select l.V3_Account_Id__c, l.Guarantor_Status_ID__c From Contact l Where l.V3_Account_Id__c = '{0}' and l.Guarantor_Status_ID__c = {1}", 
					contactId, status);

			var result = client.query(sessionHeader, null, null, null, query);

			if (result == null || result.records == null) throw new Exception(string.Format("Unable to retrieve contact by id={0}", contactId));

			return result.records.FirstOrDefault() as Contact;
		}

        public Bank_Account__c GetBankAccountById( Guid bankAccountId)
        {
            string sessionId;
            SoapClient client = Login(out sessionId);
            var sessionHeader = new SessionHeader { sessionId = sessionId };

            var query =
                String.Format(@"Select b.Account_Number__c, b.Account_Open_Date__c, b.Bank_Name__c, b.Country_Code__c, b.Holder_Name__c, 
                              b.Sort_Code__c, b.V3_Bank_Account_ID__c
                              From Bank_Account__c b 
                              Where b.V3_Bank_Account_ID__c = '{0}'",
                              bankAccountId);

            QueryResult result = client.query(sessionHeader, null, null, null, query);

			if (result == null || result.records == null) throw new Exception(string.Format("Unable to retrieve bank account by id={0}", bankAccountId));

            return result.records.FirstOrDefault() as Bank_Account__c;
        }

        public Billing_Card__c GetPaymentCardById(Guid paymentCardId, string customCondition)
        {
            SessionHeader sessionHeader;
            SoapClient client = Login(out sessionHeader);

            var query =
                String.Format(@"Select p.V3_Payment_Card_ID__c, p.V3_Billing_Address_Id__c, p.Type__c, p.Town__c, p.Start_Date__c, 
                              p.Post_Code__c, p.Masked_Number__c, p.Issue_No__c, p.Holder_Name__c, p.Expiry_Date__c, p.County__c,
                              p.Country__c, p.Address_Line_2__c, p.Address_Line_1__c, p.Customer_Account__r.V3_Organization_Id__c
                              From Billing_Card__c p 
                              Where p.V3_Payment_Card_ID__c = '{0}' ",
                              paymentCardId);
            if(!string.IsNullOrEmpty(customCondition))
            {
                query += customCondition;
            }
            QueryResult result = client.query(sessionHeader, null, null, null, query);

            if (result == null || result.records == null) throw new Exception(string.Format("Unable to retrieve payment card by id={0}", paymentCardId));

            return result.records.FirstOrDefault() as Billing_Card__c;
        }
        
         public IEnumerable<Loan_Application__History> GetApplicationHistoryById(Guid applicationId, string fieldFilter = null)
        {
            if (!String.IsNullOrEmpty(fieldFilter))
            {
                return GetApplicationHistoryById(applicationId, new string[] { fieldFilter });
            }

            return GetApplicationHistoryById(applicationId, (IEnumerable<string>)null);
        }

        public IEnumerable<Loan_Application__History> GetApplicationHistoryById(Guid applicationId, IEnumerable<string> fieldFilter)
        {
            string sessionId;
            SoapClient client = Login(out sessionId);
            SessionHeader sessionHeader = new SessionHeader { sessionId = sessionId };

            string sfIdQuery = String.Format("Select l.Id From Loan_Application__c l Where l.V3_Application_Id__c = '{0}'", applicationId);

            QueryResult sfIdQueryRes = client.query(sessionHeader, null, null, null, sfIdQuery);

            if (sfIdQueryRes == null || sfIdQueryRes.records == null)
            {
                throw new Exception(string.Format("Unable to retrieve Salesforce Id for loan application by V3 id={0}", applicationId));
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Select l.CreatedById, l.CreatedDate, l.Field, l.IsDeleted, l.NewValue, l.OldValue, l.ParentId From Loan_Application__History l Where ParentId = '{0}' ",
                                sfIdQueryRes.records.First().Id);

            if (fieldFilter != null)
            {
                foreach (string filter in fieldFilter)
                {
                    sb.AppendFormat(" AND l.Field = '{0}'", filter);
                }
            }

            QueryResult appHistory = client.query(sessionHeader, null, null, null, sb.ToString());

            if (appHistory == null || appHistory.records == null)
            {
                throw new Exception(String.Format("Unable to retrieve the history for the application with V3 Id={0}", applicationId));
            }

            return (from r in appHistory.records select r as Loan_Application__History).AsEnumerable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salesforceContactId"></param>
        /// <param name="parentType">Can be WhatId (for accounts) or WhoId(for Contacts/Leads)</param>
        /// <param name="taskType"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public IEnumerable<Task> GetTask(string salesforceContactId,string parentType, string taskType, string subject)
        {
            SessionHeader sessionHeader;
            SoapClient client = Login(out sessionHeader);

            var query =
                String.Format(@"Select t.ActivityDate, t.WhatId, t.Type, t.Subject
                              From Task t 
                              Where t.{3} = '{0}' AND t.Type = '{1}' AND t.Subject = '{2}'",
                              salesforceContactId, taskType, subject,parentType);

            QueryResult result = client.query(sessionHeader, null, null, null, query);


            return (from r in result.records select r as Task).AsEnumerable();
        }
        
        public Contact GetContactByAccountId(string accountId)
        {
            SessionHeader sessionHeader;
            SoapClient client = Login(out sessionHeader);

            var query =
                String.Format(@"Select p.Birthdate,p.CCIN__c,p.Email,p.FirstName,p.Guarantor_Status_ID__c,p.HomePhone,p.Is_Primary_Applicant__c,
                              p.LastName,p.MailingCity,p.MailingCountry,p.MailingPostalCode,p.MailingState,p.MailingStreet,p.MobilePhone,p.PO_Box__c,
                              p.Phone,p.V3_Account_Id__c
                              From Contact p 
                              Where p.V3_Account_Id__c = '{0}'",
                              accountId);

            QueryResult result = client.query(sessionHeader, null, null, null, query);

            if (result == null || result.records == null) return null;

            var contact = result.records.FirstOrDefault() as Contact;

            return contact;
        }

        private string GetAllPropertyNames(string prefix,Type targetType)
        {
            StringBuilder sb = new StringBuilder();
            var properties = targetType.GetProperties().Where(p => !p.Name.EndsWith("Specified") 
                && !p.PropertyType.FullName.StartsWith("Wonga.QA.Framework.ThirdParties.SalesforceApi"));

            foreach (var property in properties)
            {
                sb.AppendFormat("{0}.{1},",prefix,property.Name);
            }

            return sb.ToString().TrimEnd(',');
        }


    }
}
