using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [Parallelizable(TestScope.All)]
    public class HsbcTests
    {
        [Test, /*AUT(AUT.Uk),*/ JIRA("UK-494")]
        public void SortCodeTableIsUpdatesOnRestartOfHsbcService()
        {
            throw new NotImplementedException("Ftp exceptions on both WIP2 and RC2, waiting for a fix");

            Table<SortCodeEntity> table = Driver.Db.BankGateway.SortCodes;
            Assert.IsNotEmpty(table);

            SortCodeEntity entity = table.First();
            entity.CreationDate = Data.GetDateTimeMin();
            table.Context.SubmitChanges();

            Driver.Svc.Hsbc.Restart();

            //TBC
        }
    }
}
