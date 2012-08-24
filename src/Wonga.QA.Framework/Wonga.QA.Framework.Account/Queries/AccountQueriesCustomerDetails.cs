using System;

namespace Wonga.QA.Framework.Account.Queries
{
    public class AccountQueriesCustomerDetails
    {
        public String GetCustomerPostCode(Guid accountId)
        {
            var cDb = Drive.Data.Comms.Db;
            return cDb.Addresses.FindByAccountId(accountId).PostCode;
        }
    }
}
