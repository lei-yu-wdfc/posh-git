using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	public class CheckpointCustomerNameIsCorrectTests
	{
        private const RiskMask TestMask = RiskMask.TESTCustomerNameIsCorrect;

		private string _surname;
		private string _forename;
		private string _middleName;
		private string _nationalNumber;
		private Date _dateOfBirth;

        [FixtureSetUp]
		public void FixtureSetUp()
		{
			switch (Config.AUT)
			{
				case AUT.Za:
					{
						_surname = "ESSACK";
						_forename = "ANITHA";
						_middleName = "DEVI";
						_nationalNumber = "5712190106083";
						_dateOfBirth = new Date(new DateTime(1957, 12, 19));
					}
					break;
				default:
					{
						throw new NotImplementedException(Config.AUT.ToString());
					}
			}
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectAccept()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithMiddleName(_middleName)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectIncorrectSurnameDeclines()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithMiddleName(_middleName)
				.WithSurname(Get.GetName())
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectIncorrectForenameDeclines()
		{
			string incorrectForename = Get.GetName();

			//Must ensure that the first letter of incorrect surname doens't match the actual forename
			while(incorrectForename.First() == _forename.First())
				incorrectForename = Get.GetName();
		
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(incorrectForename)
				.WithMiddleName(_middleName)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectIncorrectNationalNumberDeclines()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithMiddleName(_middleName)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(Get.GetNIN(_dateOfBirth, true))
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectFirstLetterOfForenameMatchesAccepts()
		{
			string incorrectForename = Get.GetName();
			incorrectForename = _forename.First() + incorrectForename;

			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(incorrectForename)
				.WithMiddleName(_middleName)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectForenameAndMiddleNameSwappedAccepts()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_middleName)
				.WithMiddleName(_forename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectMaidenNameMatchedAccepts()
		{
			var incorrectSurname = "Incorrectsurname";

			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithMiddleName(_middleName)
				.WithSurname(incorrectSurname)
				.WithMaidenName(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectIncorrectSurnameAndMaidenNameDeclines()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_forename)
				.WithMiddleName(_middleName)
				.WithSurname(Get.GetName())
				.WithMaidenName(Get.GetName())
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectMaidenNameMatchedAndForenameAndSurnameSwappedDeclines()
		{
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(_middleName)
				.WithMiddleName(_forename)
				.WithSurname(Get.GetName())
				.WithMaidenName(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}
		
	}
}
