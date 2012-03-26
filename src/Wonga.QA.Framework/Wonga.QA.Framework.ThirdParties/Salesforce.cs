using System;
using System.ServiceModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using Wonga.QA.Framework.ThirdParties.SalesforceApi;


namespace Wonga.QA.Framework.ThirdParties
{
    public class Salesforce
    {
        public string SalesforceUsername { get; set; }
        public string SalesforcePassword { get; set; }
        public string SalesforceUrl { get; set; }

        public Salesforce()
        {
            // default
            SalesforceUsername = "datareplicator@wonga.com.wip";
            SalesforcePassword = "d33psp@c3n1n30hnAAc77NHb48XiYfzQvl3kxf";
            SalesforceUrl = "https://test.salesforce.com/services/Soap/c/22.0/0DFL00000004CC7";
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
            sessionId = null;
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
            sessionId = loginResult.sessionId;
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
                              "l.Application_Date__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}'",
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
							  "l.Application_Date__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}' and l.Customer_Account__r.V3_Organization_Id__c = '{1}'",
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
    }
}
