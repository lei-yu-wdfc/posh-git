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
	class CheckpointCustomerNameIsCorrectTests
	{
		private const string TestMask = "test:CustomerNameIsCorrect";

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
				.WithSurname("Smithee")
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectIncorrectForenameDeclines()
		{
			string incorrectForename = Data.GetName();

			//Must ensure that the first letter of incorrect surname doens't match the actual forename
			while(incorrectForename.First() == _forename.First())
				incorrectForename = Data.GetName();
		
			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(incorrectForename)
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
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(Data.GetNIN(_dateOfBirth, true))
				.Build();

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectFirstLetterOfForenameMatchesAccepts()
		{
			string incorrectForename = Data.GetName();
			incorrectForename = _forename.First() + incorrectForename;

			var customer = CustomerBuilder.New()
				.WithEmployer(TestMask)
				.WithForename(incorrectForename)
				.WithSurname(_surname)
				.WithDateOfBirth(_dateOfBirth)
				.WithNationalNumber(_nationalNumber)
				.Build();

			ApplicationBuilder.New(customer).Build();
		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectForenameAndMiddleNameSwappedAccepts()
		{

		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectMaidenNameMatchedAccepts()
		{

		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectIncorrectSurnameAndMaidenNameDeclines()
		{

		}

		[Test, AUT(AUT.Za)]
		public void CheckpointCustomerNameIsCorrectMaidenNameMatchedAndForenameAndSurnameSwappedDeclines()
		{

		}
		
	}
}
