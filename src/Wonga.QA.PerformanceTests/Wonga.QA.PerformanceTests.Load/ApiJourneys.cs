using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api.Requests.Comms.Queries;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;

namespace Wonga.QA.PerformanceTests.Load
{
	public class ApiJourneys
	{
		[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za)]
		public void L0JourneyAccepted()
		{
			new LoadRunner {
				Concurrency = 10,
				Duration = TimeSpan.FromSeconds(600),
				Test = () =>
				{
					Customer cust = CustomerBuilder.New().Build();
					ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
				} }.Run();
		}

		[Test]
		public void FileStorageGeneratePad()
		{
			Application app = null;

			Customer cust = CustomerBuilder.New().Build();
			app = ApplicationBuilder.New(cust).WithOutSigning().Build();

			new LoadRunner
			{
				Concurrency = 10,
				Duration = TimeSpan.FromSeconds(600),
				ThinkTime = TimeSpan.FromSeconds(1),
				Test = () =>
				{
					var m = new Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions.IWantToCreateAndStorePreAgreementDoc
					{
						ApplicationId = app.Id,
						OriginatingSagaId = Guid.NewGuid()
					};

					Drive.Msmq.FileStorage.Send(m);
				}
			}.Run();
		}

		[Test]
		public void QueryCustomerDetails20Concurrent60Seconds()
		{
			var accountId = Guid.NewGuid();
			var requests = new List<ApiRequest>
				{
					CreateAccountCommand.New(
						r =>
							{
								r.AccountId = accountId;
								r.Login = Get.RandomEmail();
								r.Password = "Passw0rd";
							})
				};

			Drive.Api.Commands.Post(requests);

			new LoadRunner
			{
				Concurrency = 20,
				Duration = TimeSpan.FromSeconds(60),
				ThinkTime = TimeSpan.FromSeconds(0),
				Test = () =>
				{
					for (int i = 0; i < 1000; i++)
					{
						var request = new GetCustomerDetailsQuery { AccountId = accountId };
						Drive.Api.Queries.Post(request);
					}
				}
			}.Run();
		}
	}
}
