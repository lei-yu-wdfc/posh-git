using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Data
{
    public class MigrationHelper
    {
        private readonly DataDriver _drive = new DataDriver();

        public string GetMigratedAccountLogin()
        {
            var opsAccounts = _drive.Ops.Db.Accounts;

            var totalUsers = opsAccounts.All().Count();

            var user = new Random();
            
            var callUser = user.Next(1, totalUsers);

            var userName = opsAccounts.FindByAccountId(callUser);

            return userName.Login;
        }

        public string GetMigratedAccountLoginPassword(string login)
        {
            var greyfaceUsers = _drive.WongaWholeStaging.Db.greyface.Users;

            var passwordHashKey = greyfaceUsers.FindByuser_name(login).password;
            
            DataSet dsDecryptedPassword = new DataSet();
            String query = "select * from [GreyfaceShell].[dbo].[GetUserPassword](@password)"; //GetUserPassword is table-valued n it can't be done with simple.Data
            SqlCommand cmd = new SqlCommand(query, new SqlConnection("Data Source='mig-int-v2.test.wonga.com';Initial Catalog=GreyfaceShell;Integrated Security=True"));
            cmd.Parameters.Add("password", SqlDbType.VarBinary).Value = passwordHashKey;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dsDecryptedPassword);

            return dsDecryptedPassword.Tables[0].Rows[0].ItemArray[0].ToString();
        }
    }
}
