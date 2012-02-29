using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
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

		private SoapClient Login(out string sessionId)
		{
			sessionId = null;
			var client = new SoapClient();
			LoginResult loginResult;

			try
			{
				string username = SalesforceUsername;
				string password = SalesforcePassword;
				client.Endpoint.Address = new EndpointAddress(SalesforceUrl);
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
			string sessionId;
			SoapClient client = Login(out sessionId);
			SessionHeader sessionHeader = new SessionHeader { sessionId = sessionId };

			var query = String.Format("Select l.V3_Application_Id__c, l.Transmission_Fee__c, l.Status__c, l.Service_Fee__c, " + 
										"l.Promise_Date__c, l.Number_Of_Weeks__c, l.Next_Due_Date__c, l.Monthly_Interest_Rate__c, " + 
										"l.Loan_Amount__c, l.Initiation_Fee__c, l.Customer_Account__c, l.CurrencyIsoCode, l.Application_Fee__c, " + 
										"l.Application_Date__c From Loan_Application__c l Where l.V3_Application_Id__c = '{0}'", applicationId);

			QueryResult result = client.query(sessionHeader, null, null, null, query);

			return result.records.Count() == 1;
		}
	}
}
