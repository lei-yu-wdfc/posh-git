using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Api.Requests.Comms.ContactManagement.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.ContactManagement
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class AddDirectorCommandTests
    {
        [Test, JIRA("SME-1128"), Description("This test validates email uniquess check of command validator"), AUT(AUT.Wb)]        
        public void TestPrimaryDirectorNonUniqueEmail()
        {
            String email = Get.RandomEmail();
            var req= AddPrimaryOrganisationDirectorCommand.New();

            req.Email = email;  

            ApiResponse resp = Drive.Api.Commands.Post(req);

            Do.With.Timeout(2).Interval(10).Until(() => Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(o=>o.Email==email)==1);

            bool errorDetected = false;
            try
            {
                resp = Drive.Api.Commands.Post(req);
            }
            catch (ValidatorException e)
            {                        
                foreach (String s in e.Errors)
                {
                    if (s == "Comms_EmailAddress_NotUnique")
                    {
                        errorDetected = true;
                    }
                }
            }
            

            Assert.IsTrue(errorDetected, "Email not unique error was expected");
        }

        [Test, JIRA("SME-1128"), Description("This test validates email uniquess check of command validator"), AUT(AUT.Wb)]        
        public void TestSecondaryDirectorNonUniqueEmail()
        {
            String email = Get.RandomEmail();
            var req = AddSecondaryOrganisationDirectorCommand.New();

            req.Email = email;

            ApiResponse resp = Drive.Api.Commands.Post(req);

            Do.With.Timeout(2).Interval(10).Until(() => Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(o => o.Email == email) == 1);

            bool errorDetected = false;
            try
            {
                resp = Drive.Api.Commands.Post(req);
            }
            catch (ValidatorException e)
            {
                foreach (String s in e.Errors)
                {
                    if (s == "Comms_EmailAddress_NotUnique")
                    {
                        errorDetected = true;
                    }
                }
            }


            Assert.IsTrue(errorDetected, "Email not unique error was expected");
        }

        [Test, JIRA("SME-1167"), Description("This test validates AcountId uniquess check of command validator"), AUT(AUT.Wb)]        
        public void TestSecondaryDirectorAccountIdNotUnique()
        {
            var accountId = Guid.NewGuid();

            var req = AddSecondaryOrganisationDirectorCommand.New();

            req.AccountId = accountId;

            ApiResponse resp = Drive.Api.Commands.Post(req);

            Do.With.Timeout(2).Interval(10).Until(() => Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(o => o.AccountId == accountId) == 1);

            bool errorDetected = false;
            try
            {
                req.Email = Get.RandomEmail();
                resp = Drive.Api.Commands.Post(req);
            }
            catch (ValidatorException e)
            {
                foreach (String s in e.Errors)
                {
                    if (s == "Comms_AccountId_NotUnique")
                    {
                        errorDetected = true;
                    }
                }
            }


            Assert.IsTrue(errorDetected, "AccountId not unique error was expected");   
        }

        [Test, JIRA("SME-1167"), Description("This test validates AcountId uniquess check of command validator"), AUT(AUT.Wb)]
        public void TestPrimaryDirectorAccountIdNotUnique()
        {
            var accountId = Guid.NewGuid();

            var req = AddPrimaryOrganisationDirectorCommand.New();

            req.AccountId = accountId;

            ApiResponse resp = Drive.Api.Commands.Post(req);

            Do.With.Timeout(2).Interval(10).Until(() => Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(o => o.AccountId == accountId) == 1);

            bool errorDetected = false;
            try
            {
                req.Email = Get.RandomEmail();
                resp = Drive.Api.Commands.Post(req);
            }
            catch (ValidatorException e)
            {
                foreach (String s in e.Errors)
                {
                    if (s == "Comms_AccountId_NotUnique")
                    {
                        errorDetected = true;
                    }
                }
            }


            Assert.IsTrue(errorDetected, "AccountId not unique error was expected");
        }
    }
}
