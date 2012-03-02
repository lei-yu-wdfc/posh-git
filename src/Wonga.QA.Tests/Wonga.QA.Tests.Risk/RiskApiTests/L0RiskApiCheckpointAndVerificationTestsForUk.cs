using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.RiskApiTests
{
	class L0RiskApiCheckpointAndVerificationTestsForUk : BaseL0RiskApiCheckpointAndVerificationTests
	{
		[Test, AUT(AUT.Uk)]
		public void GivenL0Applicant_WhenCustomerIsUnEmployed_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);
			CustomerBuilder builder = CustomerBuilder.New()
				.WithEmployerStatus("Unemployed").WithEmployer("test:EmployedMask");
			L0ApplicationWithSingleCheckPointAndSingleVerification(builder, CheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
		}


		[Test, AUT(AUT.Uk)]
		public void GivenL0Applicant_WhenCustomerIsEmployed_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();
			L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.CustomerIsEmployed, "CustomerIsEmployedVerification");
		}

		[Test, AUT(AUT.Uk)]
		public void GivenL0Applicant_WhenIsUnderAged_ThenIsDeclined()
		{
			_builderConfig = new ApplicationBuilderConfig(ApplicationDecisionStatusEnum.Declined);
			CustomerBuilder builder = CustomerBuilder.New()
				.WithEmployer("test:ApplicantIsNotMinor")
				.WithDateOfBirth(new Date(DateTime.Now.AddYears(-18), DateFormat.Date));
			L0ApplicationWithSingleCheckPointAndSingleVerification(
				builder, CheckpointDefinitionEnum.ApplicantIsNotMinor,
				"ApplicantIsNotMinorVerification");
		}

		[Test, AUT(AUT.Uk)]
		public void GivenL0Applicant_WhenCustomerIsNotMinor_ThenIsAccepted()
		{
			_builderConfig = new ApplicationBuilderConfig();
			L0ApplicationWithSingleCheckPointAndSingleVerification(CheckpointDefinitionEnum.ApplicantIsNotMinor, "ApplicantIsNotMinorVerification");
		}
	}
}
