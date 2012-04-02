using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    public class CheckpointGuarantorNameMatchMainApplicant
    {
        [Test, JIRA("SME-1171"), AUT(AUT.Wb)]
        public void GuarantorNameMatchMainApplicant_LoanIsDeclined()
        {
            var mainApplicantForename = Get.RandomString(8);
            var mainApplicantSurname = Get.RandomString(8);
            var mainApplicantDateOfBirth = Get.GetDoB();

            var mainApplicant =
                CustomerBuilder.New().WithForename(mainApplicantForename).WithSurname(mainApplicantSurname).WithMiddleName(RiskMask.TESTNoCheck).
                    WithDateOfBirth(mainApplicantDateOfBirth).Build();

            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New().WithForename(mainApplicantForename).WithSurname(mainApplicantSurname).WithDateOfBirth(mainApplicantDateOfBirth),
                                    };

            //CreateApplicationWithAsserts
            var organisation = OrganisationBuilder.New(mainApplicant).Build();
            var applicationBuilder = ((BusinessApplicationBuilder)ApplicationBuilder.New(mainApplicant, organisation)).WithGuarantors(guarantorList);
            //var application = applicationBuilder.Build();

            //Comms will throw an exception here
            var error =  Assert.Throws<ValidatorException>(() => applicationBuilder.Build());
            Assert.Contains(error.Message, "Comms_Customer_Recognised");
        }
    }
}
