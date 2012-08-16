﻿using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Account.PayLater;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Application;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public abstract class PayLaterApplicationBuilderBase
	{
		protected Guid ApplicationId { get; private set; }
		protected PayLaterApplicationDataBase ApplicationData { get; private set; }
		protected PayLaterAccount Account { get; private set; }


		protected PayLaterApplicationBuilderBase(PayLaterAccount account, PayLaterApplicationDataBase applicationData)
		{
			ApplicationId = Guid.NewGuid();
			Account = account;
			ApplicationData = applicationData;
		}

		public PayLaterApplication Build()
		{
			CreateApplication();
			WaitForApplicationDecision();
			WaitForApplicationToBecomeLive();

			return new PayLaterApplication(ApplicationId);
		}

		private void CreateApplication()
		{
			var commands = new List<ApiRequest>();
			commands.AddRange(GetGenericApiCommands());
			commands.AddRange(GetRegionSpecificApiCommands());
			Drive.Api.Commands.Post(commands);
		}

		private IEnumerable<ApiRequest> GetGenericApiCommands()
		{
			throw new NotImplementedException();
		}

		protected abstract IEnumerable<ApiRequest> GetRegionSpecificApiCommands();

		protected abstract void WaitForApplicationToBecomeLive();

		private void WaitForApplicationDecision()
		{
			Do.Until(() => GetApplicationDecision() == ApplicationData.ExpectedDecision);
		}

		private ApplicationDecisionStatus GetApplicationDecision()
		{
			throw new NotImplementedException();
		}

		#region "With" Methods
		#endregion
	}
}
