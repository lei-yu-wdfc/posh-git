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
        //private static List<dynamic> checkedUsers = new List<dynamic>();

        private int GetTotalUserInOpsAccounts(string forYear = null)
        {
            var opsAccounts = _drive.Ops.Db.Accounts;
            var totalUsers = 0;
            var year = Convert.ToDateTime("01/01/" + forYear);

            totalUsers = string.IsNullOrEmpty(forYear) ? opsAccounts.All().Count() : opsAccounts.All().Where(opsAccounts.CreatedOn >= year).Count();

            return totalUsers;
        }


        public string GetMigratedAccountLogin(dynamic callUser = null, string forYear = null)
        {
            var opsAccounts = _drive.Ops.Db.Accounts;
            var totalUsers = GetTotalUserInOpsAccounts(forYear);


            if (callUser == null || Convert.ToInt64(callUser) > Convert.ToInt64(totalUsers) || Convert.ToInt64(callUser) == 0)
            {
                var user = new Random();
                callUser = user.Next(1, totalUsers);

          /*      if (checkedUsers.Count > 0)
                {
                    while (checkedUsers.Contains(callUser))
                    {
                        callUser = user.Next(1, totalUsers);
                    }
                }
                checkedUsers.Add(callUser);*/
            }

            var userName = opsAccounts.FindByAccountId(callUser);

             return userName.Login;
        }

        public List<int> GetListOfMigratedAccounts(string year = null)
        {
            if (!string.IsNullOrEmpty(year))
            {

            }
            return null;
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
