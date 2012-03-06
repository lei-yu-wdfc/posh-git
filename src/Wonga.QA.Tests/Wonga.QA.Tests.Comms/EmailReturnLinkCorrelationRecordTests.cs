using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    public class EmailReturnLinkCorrelationRecordTests
    {
        [Test,AUT(AUT.Wb), JIRA("SME-976"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForGuarantorDocumentsAndEmailGenerated()
        {
            Customer cust = CustomerBuilder.New().Build();

            var organisationBuilder = OrganisationBuilder.New();
            Organisation comp = organisationBuilder.WithPrimaryApplicant(cust).WithSoManySecondaryDirectors(2).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(cust, comp) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();
            organisationBuilder.BuildSecondaryDirectors();
            businessApplicationBuilder.BuildForSecondaryDirectors();


            DoBuilder _do = new DoBuilder(new TimeSpan(0,2,0), new TimeSpan(0,0,20));
            var emailCorrelationRecords = _do.Until(() => Driver.Db.Comms.EmailReturnLinkCorrelationWbUks.Count(p => p.OrganisationId == comp.Id) == 2);
            var guarantorGuaranteesDocs = _do.Until(() => Driver.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.DocumentType == 12) == 2);
        }
    }
}
