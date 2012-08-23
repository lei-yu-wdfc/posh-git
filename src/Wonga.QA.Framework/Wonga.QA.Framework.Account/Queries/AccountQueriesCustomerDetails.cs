using System;

namespace Wonga.QA.Framework.Account.Queries
{
    public class AccountQueriesCustomerDetails
    {
        public String GetCustomerPostCode(AccountBase account)
        {
            var cDb = Drive.Data.Comms.Db;
            return cDb.Addresses.FindByAccountId(account.Id).PostCode;
        }
    }
}
