using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo;

namespace Wonga.QA.PerformanceTests.Core
{
    public class DbOperations
    {
        public String BackupFolder = @"C:\Users\francis.chelladurai\DatabaseBackups";
        public String DatabaseServer = @"localhost";

        #region DatabaseNames To be BackedUp
        public String[] Databases = new[] {"Accounting", "BankValidate",
                                            "Bi", "BiCustomerManagement", "BlackList", "CardPayment",
                                            "ColdStorage", "Comms", "Experian", "ExperianBulk",
                                            "Marketing", "Ops", "OpsLogs", "OpsSagas", "PayLater",
                                            "Payments", "PrepaidCard", /*"ReportServer",*/ "ReportServerTempDB",
                                            "Risk", "SalesForce", "Sms"};
        #endregion

        #region Connection Objects
        readonly OleDbConnection connection = new OleDbConnection();
        private Server MyServer;
        #endregion

        #region Backup/Restore
        /// <summary>
        /// Establish a server connection using Windows Authentication
        /// </summary>
        public void EstablishServerConnection()
        {
            MyServer = new Server(DatabaseServer);
            MyServer.ConnectionContext.LoginSecure = true;
            MyServer.ConnectionContext.Connect();
        }

        /// <summary>
        /// Terminate server connection
        /// </summary>
        public void TerminateServerConnection()
        {
            if (MyServer.ConnectionContext.IsOpen)
                MyServer.ConnectionContext.Disconnect();
        }

        /// <summary>
        /// Backup the given Database
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="fileName"></param>
        public void DoBackupDatabase(string databaseName, String fileName)
        {
            var bkpDbFull = new Backup {Action = BackupActionType.Database, Database = databaseName};

            bkpDbFull.Devices.AddDevice(fileName, DeviceType.File);
            bkpDbFull.BackupSetName = databaseName + " database Backup";
            bkpDbFull.BackupSetDescription = databaseName + " database - Full Backup";
            bkpDbFull.Initialize = false;
            
            bkpDbFull.SqlBackup(MyServer);
        }

        /// <summary>
        /// Restore the database using given backup file
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="fileName"></param>
        public void DoRestoreDatabase(string databaseName, string fileName)
        {
            var restoreDb = new Restore {Database = databaseName, Action = RestoreActionType.Database};

            restoreDb.Devices.AddDevice(fileName, DeviceType.File);
            restoreDb.ReplaceDatabase = true;
            restoreDb.NoRecovery = false;

            restoreDb.SqlRestore(MyServer);
        }
        #endregion

        #region Schema Operations
        /// <summary>
        /// Connect to Database use this method for doing a Scema check and Retrieve Data
        /// </summary>
        public void ConnectToDatabase()
        {
            try
            {
                connection.ConnectionString = "Provider=SQLOLEDB;Data Source=" + DatabaseServer + ";Database=Accounting;Trusted_Connection=yes;";
                connection.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error in connecting to database.. " + e.Message + "::" + e.StackTrace);
            }
        }

        /// <summary>
        /// Disconnect From Database
        /// </summary>
        public void DisConnectFromDatabase()
        {
            if (connection!=null)
                connection.Close();
        }

        /// <summary>
        /// Get the primary keys list from Database
        /// </summary>
        public DataTable GetTablePrimaryKeys(string databaseName, string tableName)
        {
            var table = GetOleSchemaTable(OleDbSchemaGuid.Primary_Keys,
                                          new object[] { databaseName, tableName.ToLower(), tableName });
            return table;
        }

        /// <summary>
        /// Returns the schema for a table
        /// </summary>
        public DataTable GetTableSchema(string tableName)
        {
            var table = GetOleSchemaTable(OleDbSchemaGuid.Tables,
                                          new Object[] { null, null, tableName, "TABLE" });
            return table;
        }

        /// <summary>
        /// Returns the schema for the Database
        /// </summary>
        public DataTable GetDatabaseSchema()
        {
            var table = GetOleSchemaTable(OleDbSchemaGuid.Tables,
                                          new Object[] { null, null, null, "TABLE"});
            return table;
        }

        /// <summary>
        /// Retruns schema table
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="restrictions"></param>
        /// <returns></returns>
        public DataTable GetOleSchemaTable(Guid schema, object[] restrictions)
        {
            return connection.GetOleDbSchemaTable(schema, restrictions);
        }

        /// <summary>
        /// Prints the schema table
        /// </summary>
        /// <param name="schemaTable"></param>
        public void PrinoutSchemaTable(DataTable schemaTable)
        {
            for (int j = 0; j < schemaTable.Rows.Count; j++)
            {
                for (int i = 0; i < schemaTable.Columns.Count; i++)
                {
                    Console.WriteLine(schemaTable.Columns[i].ToString() + " : " +
                                      schemaTable.Rows[j][i].ToString());
                }
            }
        }
        #endregion

        #region Database Operations
        /// <summary>
        /// Deeletes the given Database
        /// </summary>
        /// <param name="databaseName"></param>
        public void DropDatabase(String databaseName)
        {
            var db = new Database(MyServer, databaseName);
            db.Refresh();
            db.Drop();
        }

        /// <summary>
        /// Creates a new Snapshot
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="snapshotName"></param>
        public void CreateDatabaseSnapshot(String databaseName, string snapshotName)
        {
            SqlConnection myConn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");
            var queryStr =
                "CREATE DATABASE "+ snapshotName + " ON " +
                "(NAME = 'datafile', FILENAME = '"+ snapshotName +".snp') " +
                "AS SNAPSHOT OF " + databaseName;
            SqlCommand command = new SqlCommand(queryStr, myConn);

            try
            {
                myConn.Open();
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine("Cannot create snpshot... " + e.Message + e.StackTrace);
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
        }
        #endregion

        [Test]
        public void TestBackUp()
        {
            EstablishServerConnection();
            /*foreach (var database in Databases)
            {
                DoBackupDatabase(database, BackupFolder + "//" + database + "Current.bak");
            }*/

            foreach (var database in Databases)
            {
                Console.WriteLine("Restoring Database ... " + database);
                DoRestoreDatabase(database, BackupFolder + "//" + database + "Current.bak");    
            }
            
            TerminateServerConnection();
        }
    }
}
