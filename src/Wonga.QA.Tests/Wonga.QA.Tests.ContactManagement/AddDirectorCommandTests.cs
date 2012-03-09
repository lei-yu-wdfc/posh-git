using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.ContactManagement
{
    [TestFixture]
    public class AddDirectorCommandTests
    {
        [Test]
        public void TestPrimaryDirectorNonUniqueEmail()
        {
            String email = Data.RandomEmail();
            var req= AddPrimaryOrganisationDirectorCommand.New();

            req.Email = email;  

            ApiResponse resp = Driver.Api.Commands.Post(req);

            bool errorDetected = false;
            try
            {
                resp = Driver.Api.Commands.Post(req);
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

        [Test]
        public void TestSecondaryDirectorNonUniqueEmail()
        {
            String email = Data.RandomEmail();
            var req = AddSecondaryOrganisationDirectorCommand.New();

            req.Email = email;

            ApiResponse resp = Driver.Api.Commands.Post(req);

            bool errorDetected = false;
            try
            {
                resp = Driver.Api.Commands.Post(req);
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
    }
}
