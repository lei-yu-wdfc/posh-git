using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api.Exceptions;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class MobilePhoneTests
    {
        [SetUp, AUT]
        public void Setup(){}

        [Test, AUT(AUT.Wb),JIRA("SME-563"), Description("This test sends mobile phone verification message to predefined number")]
        public void TestVerifyMobilePhoneCommand()
        {
            var verificationId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            const string mobilePhone = "07200000000";

            Assert.DoesNotThrow(() => Driver.Api.Commands.Post(new VerifyMobilePhoneUkCommand()
                              {
                                  AccountId = accountId,
                                  Forename = Data.RandomString(8),
                                  MobilePhone = mobilePhone,
                                  VerificationId = verificationId
                              }));
            var mobileVerificationEntity = Do.Until(() => Driver.Db.Comms.MobilePhoneVerifications.SingleOrDefault(p => p.VerificationId == verificationId));
            Assert.IsNotNull(mobileVerificationEntity);
            Assert.AreEqual(mobileVerificationEntity.AccountId, accountId, "These values should be equal");
            Assert.AreEqual(mobileVerificationEntity.MobilePhone, mobilePhone, "These values should be equal");
        }

        [Test, AUT(AUT.Wb),JIRA("SME-563"), Description("This negative test attempts to send verification sms to invalid UK phone numbers and checks for expected failure")]
        public void TestVerifyMobilePhoneCommand_WithInvalidNumber()
        {
            const string invalidMobilePhone = "072000000000";
            var exceptionObject = Assert.Throws<ValidatorException>(() => Driver.Api.Commands.Post(new VerifyMobilePhoneUkCommand()
                                                                                         {
                                                                                             AccountId = Guid.NewGuid(),
                                                                                             Forename =
                                                                                                 Data.RandomString(8),
                                                                                             MobilePhone =
                                                                                                 invalidMobilePhone,
                                                                                             VerificationId =
                                                                                                 Guid.NewGuid()
                                                                                         }));

            Assert.AreEqual(exceptionObject.Errors.ToList()[0], "Ops_RequestXmlInvalid", "These values should be equal");
        }

        [Test, AUT(AUT.Wb),JIRA("SME-563"), Description("This test covers the process of creating a new customer details record, initiates mobile phone verification and completes it with correct PIN")]
        public void CompleteMobilePhoneVerification()
        {
            var accountId = Guid.NewGuid();
            var verificationId = Guid.NewGuid();
            const string mobilePhoneNumber = "07712345678";
            var commsDb = Driver.Db.Comms;
            var newEntity = new CustomerDetailEntity
            {
                AccountId = accountId,
                Gender = 2,
                DateOfBirth = Data.GetDoB(),
                Email = Data.GetEmail(),
                Forename = Data.RandomString(8),
                Surname = Data.RandomString(8),
                MiddleName = Data.RandomString(8),
                HomePhone = "0217050520",
                WorkPhone = "0217450510",
                MobilePhone = mobilePhoneNumber
            };

            commsDb.CustomerDetails.InsertOnSubmit(newEntity);
            commsDb.SubmitChanges();

            Assert.DoesNotThrow(() => Driver.Api.Commands.Post(new VerifyMobilePhoneUkCommand()
                                          {
                                              AccountId = accountId,
                                              Forename = Data.RandomString(8),
                                              MobilePhone = mobilePhoneNumber,
                                              VerificationId = verificationId
                                          }));
            var mobileVerificationEntity =
                Do.Until(() => Driver.Db.Comms.MobilePhoneVerifications.SingleOrDefault(p => p.VerificationId == verificationId));
            Assert.IsNotNull(mobileVerificationEntity);
            Assert.IsNotNull(mobileVerificationEntity.Pin);

            Assert.DoesNotThrow(() => Driver.Api.Commands.Post(new CompleteMobilePhoneVerificationCommand()
                                                                 {
                                                                     Pin = mobileVerificationEntity.Pin,
                                                                     VerificationId = verificationId
                                                                 }));
        }

        [Test, AUT(AUT.Wb),JIRA("SME-563"), Description("This negative test covers the process of creating a new customer details record, initiates mobile phone verification and completes it with incorrect PIN, while checking for failure")]
        public void TestCompleteEmailVerificationCommand_WithInvalidPin()
        {
            var exceptionObject = Assert.Throws<ValidatorException>(() => Driver.Api.Commands.Post(new CompleteMobilePhoneVerificationCommand()
                                                             {
                                                                 Pin = "",
                                                                 VerificationId = Guid.NewGuid()
                                                             }));

            Assert.AreEqual(exceptionObject.Errors.ToList()[0], "Ops_RequestXmlInvalid", "These values should be equal");
        }

        [Test, AUT(AUT.Wb),JIRA("SME-563"), Description("This test initiates the resend pin feature for new customer details record")]
        public void TestResendMobilePhonePin()
        {
            var accountId = Guid.NewGuid();
            var verificationId = Guid.NewGuid();
            const string mobilePhoneNumber = "07712345678";
            var commsDb = Driver.Db.Comms;
            var newEntity = new CustomerDetailEntity
            {
                AccountId = accountId,
                Gender = 2,
                DateOfBirth = Data.GetDoB(),
                Email = Data.GetEmail(),
                Forename = Data.RandomString(8),
                Surname = Data.RandomString(8),
                MiddleName = Data.RandomString(8),
                HomePhone = "0217050520",
                WorkPhone = "0217450510",
                MobilePhone = mobilePhoneNumber
            };

            commsDb.CustomerDetails.InsertOnSubmit(newEntity);
            commsDb.SubmitChanges();

            Assert.DoesNotThrow(() => Driver.Api.Commands.Post(new VerifyMobilePhoneUkCommand()
            {
                AccountId = accountId,
                Forename = Data.RandomString(8),
                MobilePhone = mobilePhoneNumber,
                VerificationId = verificationId
            }));
            var mobileVerificationEntity = Do.Until(() => Driver.Db.Comms.MobilePhoneVerifications.SingleOrDefault(p => p.VerificationId == verificationId));
            Assert.IsNotNull(mobileVerificationEntity);

            Assert.DoesNotThrow(()=>Driver.Api.Commands.Post(new ResendMobilePhonePinCommand()
                                                                 {
                                                                     Forename = Data.RandomString(8),
                                                                     VerificationId = mobileVerificationEntity.VerificationId
                                                                 }));
        }

    }
}
