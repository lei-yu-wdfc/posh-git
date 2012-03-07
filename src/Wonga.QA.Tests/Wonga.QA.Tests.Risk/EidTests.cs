using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
    public class EidTests
    {
        CustomerBuilder _customerBuilder;
        Customer _customer;
        private const int SessionOne = 1;
        private const int SessionTwo = 2;
        private const int SessionThree = 3;

        [SetUp]
        public void Setup()
        {
            const string forename = "Plane";
            const string surname = "Testpal";

            _customerBuilder = CustomerBuilder.New();

            _customerBuilder.ScrubForename(forename);
            _customerBuilder.ScrubSurname(surname);

            const string middleName = "TESTDirectFraud";
            const string employerName = "Wonga";
            var email = Data.GetEmailWithoutPlusChar();

            var dateOfBirth = new Date(new DateTime(1981, 11, 11), DateFormat.Date);
            const string nationalNumber = "811224575";
            const string phoneNumber = "9052000102";
            const string houseNumber = "71";
            const string street = "STATIONVIEW PL";
            const string city = "Bolton";
            const string postCode = "L7E1K9";

            _customerBuilder.WithForename(forename);
            _customerBuilder.WithMiddleName(middleName);
            _customerBuilder.WithSurname(surname);
            _customerBuilder.WithFlatInAddress(null);
            _customerBuilder.WithEmailAddress(email);
            _customerBuilder.WithEmployer(employerName);
            _customerBuilder.WithDateOfBirth(dateOfBirth);
            _customerBuilder.WithNationalNumber(nationalNumber);
            _customerBuilder.WithPhoneNumber(phoneNumber);
            _customerBuilder.WithHouseNumberInAddress(houseNumber);
            _customerBuilder.WithStreetInAddress(street);
            _customerBuilder.WithTownInAddress(city);
            _customerBuilder.WithPostcodeInAddress(postCode);
            _customerBuilder.WithCountyInAddress(null);
            _customerBuilder.WithDistrictInAddress(null);
            _customer = _customerBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_TwoCorrectAnswers_Accepted()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {{SessionOne, new List<bool>(new bool[] {true, true})}};

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_TwoCorrectAnswers_Accepted()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, true})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_OneCorrectAnswer_TwoCorrectAnswers_Accepted()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, false})},
                                                {SessionThree, new List<bool>(new bool[] {true, true})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_OneCorrectAnswer_OneCorrectAnswer_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, false})},
                                                {SessionThree, new List<bool>(new bool[] {true, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_OneCorrectAnswer_ZeroCorrectAnswers_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, false})},
                                                {SessionThree, new List<bool>(new bool[] {false, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_ZeroCorrectAnswers_TwoCorrectAnswer_Accepted()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {false, false})},
                                                {SessionThree, new List<bool>(new bool[] {true, true})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_ZeroCorrectAnswers_OneCorrectAnswer_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {false, false})},
                                                {SessionThree, new List<bool>(new bool[] {true, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_OneCorrectAnswer_ZeroCorrectAnswers_ZeroCorrectAnswers_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {true, false})},
                                                {SessionTwo, new List<bool>(new bool[] {false, false})},
                                                {SessionThree, new List<bool>(new bool[] {false, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_ZeroCorrectAnswers_ThreeCorrectAnswers_Accepted()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {false, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, true, true})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_ZeroCorrectAnswers_TwoCorrectAnswers_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {false, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, true, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_ZeroCorrectAnswers_OneCorrectAnswer_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {false, false})},
                                                {SessionTwo, new List<bool>(new bool[] {true, false, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1647")]
        public void Eid_ZeroCorrectAnswers_ZeroCorrectAnswers_Declined()
        {
            var eidSessionInteraction = new Dictionary<int, List<bool>>
                                            {
                                                {SessionOne, new List<bool>(new bool[] {false, false})},
                                                {SessionTwo, new List<bool>(new bool[] {false, false, false})}
                                            };

            ApplicationBuilder applicationBuilder =
                ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatusEnum.Declined).
                    WithEidSessionInteraction(eidSessionInteraction);
            applicationBuilder.Build();
        }
    
        [TearDown]
        public void TearDown()
        {
            _customer.UpdateForename(Data.GetName());
            _customer.UpdateSurname(Data.GetName());
        }
    }
}
