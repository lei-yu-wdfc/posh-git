﻿using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Address.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Comms.Queries;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api.Exceptions;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class CustomerTests
    {
        [Test, AUT(AUT.Wb,AUT.Uk), Owner(Owner.StanDesyatnikov)]
        [JIRA("SME-561"), Description("This test verifies the save customer address command by verifying its response and verifying that the record has been created in the repository")]
        public void TestAddressOverridesCorrectlySaved()
        {
            var customer = CustomerBuilder.New()
                 .WithCountyInAddress("MyCounty")
                 .WithDistrictInAddress("MyDistrict")
                 .WithFlatInAddress("MyFlat")
                 .WithHouseNameInAddress("MyHouse")
                 .WithHouseNumberInAddress("MyHouseNumber")
                 .WithMiddleName("John")
                 .WithStreetInAddress("MyStreet")
                 .WithTownInAddress("MyTown").Build();

            var addressTab = Drive.Data.Comms.Db.Addresses;
            var addressEntity = Do.With.Timeout(2).Until(() => addressTab.FindAll(addressTab.AccountId == customer.Id).Single());
            Assert.AreEqual(addressEntity.Street, "MyStreet", "These values should be equal");
            Assert.AreEqual(addressEntity.County, "MyCounty", "These values should be equal");
            Assert.AreEqual(addressEntity.District, "MyDistrict", "These values should be equal");
            Assert.AreEqual(addressEntity.Flat, "MyFlat", "These values should be equal");
            Assert.AreEqual(addressEntity.HouseName, "MyHouse", "These values should be equal");
            Assert.AreEqual(addressEntity.HouseNumber, "MyHouseNumber", "These values should be equal");
            Assert.AreEqual(addressEntity.Town, "MyTown", "These values should be equal");
        }

        [Test, AUT(AUT.Wb,AUT.Uk), Owner(Owner.StanDesyatnikov)]
        [JIRA("SME-561"), Description("This test verifies update customer address command by issuing a command and verifying its successful response and that the record in the repository has been changed")]
        public void TestUpdateCustomerAddressCommand()
        {
            var customer = CustomerBuilder.New()
                 .WithCountyInAddress("MyCounty")
                 .WithDistrictInAddress("MyDistrict")
                 .WithFlatInAddress("MyFlat")
                 .WithHouseNameInAddress("MyHouse")
                 .WithHouseNumberInAddress("MyHouseNumber")
                 .WithMiddleName("John")
                 .WithStreetInAddress("MyStreet")
                 .WithTownInAddress("MyTown").Build();

            var addressTab = Drive.Data.Comms.Db.Addresses;
            var addressEntity = Do.With.Timeout(2).Until(() => addressTab.FindAll(addressTab.AccountId == customer.Id).Single());
            var externalId = addressEntity.ExternalId;
            var message = new UpdateCustomerAddressUkCommand()
                              {
                                  AccountId = customer.Id,
                                  AddressId = externalId,
                                  AtAddressFrom = DateTime.Today.AddYears(-4).ToDate(DateFormat.Date),
                                  AtAddressTo = null,
                                  Flat = "NewMyFlat",
                                  HouseName = "NewMyHouse",
                                  HouseNumber = "NewMyHouseNumber",
                                  District = "NewMyDistrict",
                                  Street = "NewMyStreet",
                                  Town = "NewMyTown",
                                  County = "NewMyCounty",
                                  CountryCode = "UK",
                                  Postcode = "SW7 7PN"
                              };

            Drive.Api.Commands.Post(message);

            addressEntity = Do.Until(() => addressTab.FindAll(addressTab.Flat == message.Flat.ToString() && addressTab.ExternalId == externalId).Single());
            Assert.AreEqual(addressEntity.AccountId, message.AccountId, "These values should be equal");
            Assert.AreEqual(addressEntity.ExternalId, message.AddressId, "These values should be equal");
            Assert.AreEqual(addressEntity.Flat, message.Flat, "These values should be equal");
            Assert.AreEqual(addressEntity.HouseName, message.HouseName, "These values should be equal");
            Assert.AreEqual(addressEntity.HouseNumber, message.HouseNumber, "These values should be equal");
            Assert.AreEqual(addressEntity.District, message.District, "These values should be equal");
            Assert.AreEqual(addressEntity.Street, message.Street, "These values should be equal");
            Assert.AreEqual(addressEntity.Town, message.Town, "These values should be equal");
            Assert.AreEqual(addressEntity.County, message.County, "These values should be equal");
            Assert.AreEqual(addressEntity.PostCode, message.Postcode, "These values should be equal");
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-561"), Description("This test verifies get current address query by creating a new customer address, issuing a query and comparing the response to repository record")]
        public void TestGetCurrentAddressQuery()
        {
            var customer = CustomerBuilder.New()
                 .WithCountyInAddress("MyCounty")
                 .WithDistrictInAddress("MyDistrict")
                 .WithFlatInAddress("MyFlat")
                 .WithHouseNameInAddress("MyHouse")
                 .WithHouseNumberInAddress("MyHouseNumber")
                 .WithMiddleName("John")
                 .WithStreetInAddress("MyStreet")
                 .WithTownInAddress("MyTown").Build();

            var addressTab = Drive.Data.Comms.Db.Addresses;
            var addressEntity = Do.With.Timeout(2).Until(() => addressTab.FindAll(addressTab.AccountId == customer.Id).Single());
            var query = new GetCurrentAddressQuery { AccountId = addressEntity.AccountId };
            var response = Drive.Api.Queries.Post(query);
            var specificFlatName = response.Values["Flat"].Single();
            Assert.AreEqual(addressEntity.Flat, specificFlatName, "These values should be equal");
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-565"), Description("This test obtains address descriptors for all UK postcode variants and validates their content")]
        public void TestGetAddressByDescriptionId()
        {
            //var postCodes = new[] {"W1A 1HQ", "M1 1AA", "B33 8TH", "CR2 6XH", "DN55 1PT", "EC1A 1BB"};
            var postCodes = new[] { "W1A 1HQ", "M1 1AA" };

            foreach (var code in postCodes)
            {
                var message = new GetAddressDescriptorsByPostCodeUkQuery { CountryCode = "UK", Postcode = code };
                var response = Drive.Api.Queries.Post(message);
                var responseDescriptionIdList = response.Values["Id"].ToList();
                var responseDescriptorDescriptionList = response.Values["Description"].ToList();

                Assert.IsNotEmpty(responseDescriptionIdList, "This collection should not be empty");
                Assert.IsNotEmpty(responseDescriptorDescriptionList, "This collection should not be empty");

                foreach (var descriptionId in responseDescriptionIdList)
                {
                    response = Drive.Api.Queries.Post(new GetAddressByDescriptorIdUkQuery { DescriptorId = descriptionId });
                    Assert.IsNotEmpty(response.Values, "This collection should not be empty");
                }
            }
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-565"), Description("This test obtains address descriptors for all UK postcode variants")]
        public void TestGetAddressDescriptorsByPostCode()
        {
            //var postCodes = new[] {"W1A 1HQ", "M1 1AA", "B33 8TH", "CR2 6XH", "DN55 1PT", "EC1A 1BB"};
            var postCodes = new[] { "W1A 1HQ", "M1 1AA" };

            foreach (var postCode in postCodes)
            {
                var message = new GetAddressDescriptorsByPostCodeUkQuery
                {
                    CountryCode = "UK",
                    Postcode = postCode
                };

                var response = Drive.Api.Queries.Post(message);
                Assert.IsNotEmpty(response.Values, "This collection should not be empty");
            }
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test verifies Save customer personal and phone details by issuing and API call, verifying its response and locating the expected record in the DB")]
        public void TestSaveCustomerDetailsCommand()
        {
            var accountId = Guid.NewGuid();
            var message = new SaveCustomerDetailsUkCommand
                              {
                                  AccountId = accountId,
                                  Gender = "Female",
                                  DateOfBirth = new DateTime(1957, 10, 30).ToDate(DateFormat.Date),
                                  Email = Get.RandomEmail(),
                                  Forename = Get.RandomString(8),
                                  Surname = Get.RandomString(8),
                                  MiddleName = Get.RandomString(8),
                                  HomePhone = Get.GetPhone(),
                                  WorkPhone = Get.GetPhone()
                              };
            Drive.Api.Commands.Post(message);

            var customerDetailsTab = Drive.Data.Comms.Db.CustomerDetails;
            var detailsEntity = Do.Until(() => customerDetailsTab.FindAll(customerDetailsTab.AccountId == accountId).Single());

            Assert.IsNotNull(detailsEntity);
            Assert.AreEqual(message.Forename, detailsEntity.Forename, "These values should be equal");
            Assert.AreEqual(message.Email, detailsEntity.Email, "These values should be equal");
            Assert.AreEqual(message.HomePhone, detailsEntity.HomePhone, "These values should be equal");
            Assert.AreEqual(message.WorkPhone, detailsEntity.WorkPhone, "These values should be equal");
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-561"), Description("This negative test validates the scenario of saving personal and phone details of customer that is recognized, this test validates error code fron error response returned by the API call")]
        [Ignore("I cannot reproduce what this test needs. I will review later")]
        public void TestSaveCustomerDetailsCommand_DuplicateCustomer()
        {
            var accountId = Guid.NewGuid();
            var emailAddress = Get.RandomEmail();
            Drive.Api.Commands.Post(new CreateAccountCommand { AccountId = accountId, Password = "Passw0rd", Login = emailAddress });
            var message = (new SaveCustomerDetailsUkCommand
                               {
                                   AccountId = accountId,
                                   Gender = "Female",
                                   DateOfBirth = new DateTime(1957, 10, 30).ToDate(DateFormat.Date),
                                   Email = emailAddress,
                                   Forename = Get.RandomString(8),
                                   Surname = Get.RandomString(8),
                                   MiddleName = Get.RandomString(8),
                                   HomePhone = "0217050520",
                                   WorkPhone = "0217450510"
                               });

            Drive.Api.Commands.Post(message);
            var customerEntity = Do.Until(() => Drive.Db.Comms.CustomerDetails.Single(p => p.AccountId == accountId));
            message.Email = Get.RandomEmail();

            var response = Drive.Api.Commands.Post(message);
            var x = response.GetErrors();
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test verifies minimum age validation by attempting to submit underage and verifies expected failure response")]
        public void TestSaveCustomerDetailsCommand_Underage()
        {
            var accountId = Guid.NewGuid();
            var message = new SaveCustomerDetailsUkCommand
            {
                AccountId = accountId,
                Gender = "Female",
                DateOfBirth = DateTime.Now.ToDate(DateFormat.Date),
                Email = Get.RandomEmail(),
                Forename = Get.RandomString(8),
                Surname = Get.RandomString(8),
                MiddleName = Get.RandomString(8),
                HomePhone = "0217050520",
                WorkPhone = "0217450510"
            };

            var error = Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(message));
            Assert.AreEqual(error.Errors.ToList()[0], "Comms_Age_BelowMinuimumAge", "These values should be equal");
        }

        [Test, AUT(AUT.Wb,AUT.Uk, AUT.Ca), Owner(Owner.RiskTeam)]
        [JIRA("SME-561"), Description("This test verifies GetCustomerDetails query by creating new customer details record, issuing the query, verifying its response and comparing the data it returns to repository record")]
        public void TestGetCustomerDetailsQuery()
        {
            var accountId = Guid.NewGuid();
            var commsDb = Drive.Data.Comms.Db.CustomerDetails;

            dynamic newEntity = new ExpandoObject();
            newEntity.AccountId = accountId;
            newEntity.newGender = 2;
            newEntity.DateOfBirth = Get.GetDoB();
            newEntity.Email = Get.RandomEmail();
            newEntity.Forename = Get.RandomString(8);
            newEntity.Surname = Get.RandomString(8);
            newEntity.MiddleName = Get.RandomString(8);
            newEntity.HomePhone = "0217050520";
			newEntity.WorkPhone = "0217450510";
            newEntity.HasAccount = 0;

            commsDb.Insert(newEntity);

            var response = Drive.Api.Queries.Post(new GetCustomerDetailsQuery()
                                                        {
                                                            AccountId = accountId
                                                        });

            Assert.AreEqual(response.Values["Email"].Single(), newEntity.Email, "These values should be equal");
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test verifies the SaveContactPreferences command by issuing it, checking its response and verifying that the repository record has been created and compare the data in the repository to expected values")]
        public void TestSaveContactPreferencesCommand()
        {
            var accountId = Guid.NewGuid();
            var message = new SaveContactPreferencesCommand()
                              {
                                  AccountId = accountId,
                                  AcceptMarketingContact = true
                              };
            Drive.Api.Commands.Post(message);
            var contactPreferencesTab = Drive.Data.Comms.Db.ContactPreferences;
            var contactPreferenceEntity = Do.Until(() => contactPreferencesTab.FindAll(contactPreferencesTab.AccountId == accountId).SingleOrDefault());
            Assert.IsNotNull(contactPreferenceEntity);
            Assert.AreEqual(contactPreferenceEntity.AccountId, message.AccountId, "These values should be equal");
            Assert.AreEqual(contactPreferenceEntity.AcceptMarketingContact, message.AcceptMarketingContact, "These values should be equal");
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-561"), Description("This test verifies the GetContactPreferences query by saving customer contact preferences and then retrieving them via the query and verifying its response")]
        public void TestGetContactPreferencesQuery()
        {
            var accountId = Guid.NewGuid();
            var message = new SaveContactPreferencesCommand()
                                  {
                                      AccountId = accountId,
                                      AcceptMarketingContact = true
                                  };
            Drive.Api.Commands.Post(message);
            var contactPreferencesTab = Drive.Data.Comms.Db.ContactPreferences;
            var saveContactPreferencesEntity = Do.Until(() => contactPreferencesTab.FindAll(contactPreferencesTab.AccountId == accountId).SingleOrDefault());
            Assert.IsNotNull(saveContactPreferencesEntity);

            var response = Drive.Api.Queries.Post(new GetContactPreferencesQuery()
                                                       {
                                                           AccountId = accountId
                                                       });
            Assert.AreEqual(response.Values["AccountId"].Single(), saveContactPreferencesEntity.AccountId.ToString(), "These values should be equal");
            Assert.AreEqual<Boolean>(bool.Parse(response.Values["AcceptMarketingContact"].Single()), saveContactPreferencesEntity.AcceptMarketingContact, "These values should be equal");
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test verifies Password reset email command to random email by issuing the command and checking its response")]
        public void TestSendPasswordResetEmailCommand()
        {
            var accountId = Guid.NewGuid();
            var emailAddress = Get.RandomEmail();
            var commsDb = Drive.Data.Comms.Db.CustomerDetails;
            dynamic newEntity = new ExpandoObject();

            newEntity.AccountId = accountId;
            newEntity.Gender = 2;
            newEntity.DateOfBirth = Get.GetDoB();
            newEntity.Email = emailAddress;
            newEntity.Forename = Get.RandomString(8);
            newEntity.Surname = Get.RandomString(8);
            newEntity.MiddleName = Get.RandomString(8);
            newEntity.HomePhone = "0217050520";
            newEntity.WorkPhone = "0217450510";
            newEntity.HasAccount = 0;

            commsDb.Insert(newEntity);

            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new SendPasswordResetEmailCommand()
                                                                   {
                                                                       Email = emailAddress,
                                                                       NotificationId = Guid.NewGuid(),
                                                                       UriMask = "123"
                                                                   }));
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test updates home phone details of the customer and checks that the repository record has been correctly updated")]
        public void TestUpdateHomePhoneCommand()
        {
            var accountId = Guid.NewGuid();
            var commsDb = Drive.Data.Comms.Db.CustomerDetails;
            dynamic newEntity = new ExpandoObject();

            newEntity.AccountId = accountId;
            newEntity.Gender = 2;
            newEntity.DateOfBirth = Get.GetDoB();
            newEntity.Email = Get.RandomEmail();
            newEntity.Forename = Get.RandomString(8);
            newEntity.Surname = Get.RandomString(8);
            newEntity.MiddleName = Get.RandomString(8);
            newEntity.HomePhone = "0217050520";
            newEntity.WorkPhone = "0217450510";
            newEntity.HasAccount = 0;

            commsDb.Insert(newEntity);

            var updateHomePhoneMessage = new UpdateHomePhoneUkCommand()
                                             {
                                                 AccountId = accountId,
                                                 HomePhone = "02071111111"
                                             };
            Drive.Api.Commands.Post(updateHomePhoneMessage);
            var detailsEntity = Do.Until(() => commsDb.FindAll(commsDb.AccountId == accountId && commsDb.HomePhone == updateHomePhoneMessage.HomePhone.ToString()).SingleOrDefault());
            Assert.IsNotNull(detailsEntity);
            Assert.AreEqual(updateHomePhoneMessage.HomePhone, detailsEntity.HomePhone, "These values should be equal");
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test verifies email verification command by issuing it and checking its response")]
        public void TestSendVerificationEmailCommand()
        {
            var accountId = Guid.NewGuid();
            var commsDb = Drive.Data.Comms.Db.CustomerDetails;
            dynamic newEntity = new ExpandoObject();

            newEntity.AccountId = accountId;
            newEntity.Gender = 2;
            newEntity.DateOfBirth = Get.GetDoB();
            newEntity.Email = Get.RandomEmail();
            newEntity.Forename = Get.RandomString(8);
            newEntity.Surname = Get.RandomString(8);
            newEntity.MiddleName = Get.RandomString(8);
            newEntity.HomePhone = "0217050520";
            newEntity.WorkPhone = "0217450510";
            newEntity.HasAccount = 0;

            commsDb.Insert(newEntity);

            var detailsEntity = Do.Until(() => commsDb.FindAll(commsDb.AccountId == accountId).SingleOrDefault());
            Assert.IsNotNull(detailsEntity);

            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new SendVerificationEmailCommand()
                                                        {
                                                            AccountId = accountId,
                                                            Email = Get.RandomEmail(),
                                                            UriFragment = "api_test"
                                                        }));
        }

        [Test, AUT(AUT.Wb,AUT.Uk)]
        [JIRA("SME-561"), Description("This test verifies completion of email verification process by issuing the CompleteEmailVerification command and checking its response")]
        public void TestCompleteEmailVerificationCommand()
        {
            var accountId = Guid.NewGuid();
            var commsDb = Drive.Data.Comms.Db.CustomerDetails;
            dynamic newEntity = new ExpandoObject();

            newEntity.AccountId = accountId;
            newEntity.Gender = 2;
            newEntity.DateOfBirth = Get.GetDoB();
            newEntity.Email = Get.RandomEmail();
            newEntity.Forename = Get.RandomString(8);
            newEntity.Surname = Get.RandomString(8);
            newEntity.MiddleName = Get.RandomString(8);
            newEntity.HomePhone = "0217050520";
            newEntity.WorkPhone = "0217450510";
            newEntity.HasAccount = 0;

            commsDb.Insert(newEntity);
            var detailsEntity = Do.Until(() => commsDb.FindAll(commsDb.AccountId == accountId).SingleOrDefault());
            Assert.IsNotNull(detailsEntity);
            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new SendVerificationEmailCommand()
            {
                AccountId = accountId,
                Email = Get.RandomEmail(),
                UriFragment = "api_test"
            }));

            var emailVerificationsTab = Drive.Data.Comms.Db.EmailVerification;

            var emailVerificationEntity = Do.Until(() => emailVerificationsTab.FindAll(emailVerificationsTab.AccountId == accountId).SingleOrDefault());

            Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new CompleteEmailVerificationCommand()
                                                                  {
                                                                      AccountId = accountId,
                                                                      ChangeId = emailVerificationEntity.ChangeId
                                                                  }));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-850"), Owner(Owner.StanDesyatnikov)]
        public void CreateCustomerTwice()
        {
            String forename = Get.RandomString(4, 8);
            String surname = Get.RandomString(5, 10);
            Date dob = Get.GetDoB();

            CustomerBuilder.New()
                .WithForename(forename)
                .WithSurname(surname)
                .WithDateOfBirth(dob)
                .Build();

            var error = Assert.Throws<ValidatorException>(() =>
            CustomerBuilder.New()
                .WithForename(forename)
                .WithSurname(surname)
                .WithDateOfBirth(dob)
                .Build());

            Assert.Contains(error.Message, "Comms_Customer_Recognised");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-850"), Owner(Owner.StanDesyatnikov)]
        public void CreateSameCustomerTwiceInverseCaseOnSurnameAndForename()
        {
            String forename = Get.RandomString(4, 8);
            String surname = Get.RandomString(5, 10);
            Date dob = Get.GetDoB();

            CustomerBuilder.New()
                .WithForename(forename)
                .WithSurname(surname)
                .WithDateOfBirth(dob)
                .Build();

            var error = Assert.Throws<ValidatorException>(() =>
            CustomerBuilder.New()
                .WithForename(Get.InvertCase(forename))
                .WithSurname(Get.InvertCase(surname))
                .WithDateOfBirth(dob)
                .Build());

            Assert.Contains(error.Message, "Comms_Customer_Recognised");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-850"), Owner(Owner.StanDesyatnikov)]
        public void CreateSimilarCustomerDiffForename()
        {
            String forename = Get.RandomString(4, 8);
            String surname = Get.RandomString(5, 10);
            Date dob = Get.GetDoB();

            CustomerBuilder.New()
                .WithForename(forename)
                .WithSurname(surname)
                .WithDateOfBirth(dob)
                .Build();

            CustomerBuilder.New()
                .WithForename("Bert")
                .WithSurname(surname)
                .WithDateOfBirth(dob)
                .Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UK-850"), Owner(Owner.StanDesyatnikov)]
        public void CreateSimilarCustomerSoundexOnSurname()
        {
            String forename = Get.RandomString(4, 8);
            String random = Get.RandomString(5, 10);
            String surname = "Zi" + random;
            String soundexSurname = "Zy" + random;
            Date dob = Get.GetDoB();

            CustomerBuilder.New()
                .WithForename(forename)
                .WithSurname(surname)
                .WithDateOfBirth(dob)
                .Build();

            var error = Assert.Throws<ValidatorException>(() =>
            CustomerBuilder.New()
                .WithForename(forename)
                .WithSurname(soundexSurname)
                .WithDateOfBirth(dob)
                .Build());

            Assert.Contains(error.Message, "Comms_Customer_Recognised");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-850"), Owner(Owner.StanDesyatnikov)]
        public void CreateTwoCustomersSameEmail()
        {
            String email = Get.RandomEmail();

            CustomerBuilder.New()
                .WithEmailAddress(email)
                .Build();

            var error = Assert.Throws<ValidatorException>(() =>
            CustomerBuilder.New()
                .WithEmailAddress(email)
                .Build());

            Assert.Contains(error.Message, "Ops_Login_AlreadyExists");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-850"), Owner(Owner.StanDesyatnikov)]
        public void CreateTwoCustomersSameEmailInvertedCase()
        {
            String email = Get.RandomEmail();

            CustomerBuilder.New()
                .WithEmailAddress(email)
                .Build();

            var error = Assert.Throws<ValidatorException>(() =>
            CustomerBuilder.New()
                .WithEmailAddress(Get.InvertCase(email))
                .Build());

            Assert.Contains(error.Message, "Ops_Login_AlreadyExists");
        }

    }
}
