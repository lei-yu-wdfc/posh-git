using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo;

namespace Wonga.QA.PerformanceTests.Core
{
    public class DbOperations
    {
        public String BackupFolder = @"C:\Users\francis.chelladurai\DatabaseBackups\";
        public String DatabaseServer = @"localhost";
        public String BackupExtension = ".bak";

        public String AppServer = "localhost";
        public String Username = null;
        public String Password = null;
        public String ManagementPath = null;

        public Database DataBaseObj;

        #region DatabaseNames
        public String[] Databases = new[] {"Accounting", "BankGateway", "BankValidate",
                                            "BI", "BlackList", "CallReport",
                                            "CallValidate", "CardPayment","ColdStorage", "Comms", 
                                            "Experian", "ExperianBulk", "FileStorage",
                                            "Marketing", "Ops", "OpsLogs", "OpsSagas", "PayLater",
                                            "Payments", "PrepaidCard", 
                                            "Risk", "SalesForce", "Sms"};
        #endregion

        #region Connection Objects
        readonly OleDbConnection _connection = new OleDbConnection();
        private Server _myServer;
        private readonly SqlConnection _sqlConnection = new SqlConnection();
        #endregion

        #region Backup/Restore
        /// <summary>
        /// Establish a server connection using Windows Authentication
        /// </summary>
        public void EstablishServerConnection()
        {
            _myServer = new Server(DatabaseServer);
            _myServer.ConnectionContext.LoginSecure = true;
            _myServer.ConnectionContext.Connect();
        }

        /// <summary>
        /// Terminate server connection
        /// </summary>
        public void TerminateServerConnection()
        {
            if (_myServer.ConnectionContext.IsOpen)
                _myServer.ConnectionContext.Disconnect();
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
            
            bkpDbFull.SqlBackup(_myServer);
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

            restoreDb.SqlRestore(_myServer);
        }
        #endregion

        #region Schema Operations
        /// <summary>
        /// Connect to Database use this method for doing a Scema check and Retrieve Data
        /// </summary>
        public void ConnectToDatabase(string dataBase)
        {
            try
            {
                _connection.ConnectionString = "Provider=SQLOLEDB;Data Source=" + DatabaseServer + ";Database="+ dataBase + ";Trusted_Connection=yes;";
                _connection.Open();
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
            if (_connection!=null)
                _connection.Close();
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
        /// Retruns Columns DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTableColumns(string tableName)
        {
            var table = GetOleSchemaTable(OleDbSchemaGuid.Columns,
                                          new Object[] { null, null, tableName, null });
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
            return _connection.GetOleDbSchemaTable(schema, restrictions);
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

        /// <summary>
        /// Returns Tables list
        /// </summary>
        /// <param name="schemaTable"></param>
        /// <returns></returns>
        public List<string> GetTables(DataTable schemaTable)
        {
            var tables = new List<string>();

            for (int j = 0; j < schemaTable.Rows.Count; j++)
            {
                var schema = "";
                for (int i = 0; i < schemaTable.Columns.Count; i++)
                {
                    var column = schemaTable.Columns[i].ToString();
                    
                    if (column.Equals("TABLE_SCHEMA"))
                        schema = schemaTable.Rows[j][i].ToString();
                    else if (column.Equals("TABLE_NAME"))
                        tables.Add(schema + '.' + schemaTable.Rows[j][i].ToString());
                }
            }
            return tables;
        } 
        #endregion

        #region Database Operations
        /// <summary>
        /// Deeletes the given Database
        /// </summary>
        /// <param name="databaseName"></param>
        public void DropDatabase(String databaseName)
        {
            DataBaseObj.Drop();
        }
        #endregion

        #region SQL
        /// <summary>
        /// Connect to the given Database on the Sql Server
        /// </summary>
        /// <param name="databaseName"></param>
        public void EstablishSqlConnection(string databaseName)
        {
            _sqlConnection.ConnectionString ="integrated security=SSPI;data source="+ DatabaseServer + ";" +
                                            "persist security info=False;initial catalog=" + databaseName;

            try
            {
                _sqlConnection.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to connect to data source. " + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// Close the Sql Connection
        /// </summary>
        public void CloseSqlConnection()
        {
            _sqlConnection.Close();
        }

        /// <summary>
        /// Insert Data in to table
        /// </summary>
        /// <param name="insert"></param>
        public void Insert(Insert insert)
        {
            EstablishSqlConnection(insert.DatabaseName);
            var command = new SqlCommand(insert.ToString(), _sqlConnection);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ae)
            {
                Console.WriteLine("Error while inserting in to table " + insert.Table);
                Console.WriteLine(ae.Message);
                Console.WriteLine(ae.StackTrace);
            }
            CloseSqlConnection();
        }

        /// <summary>
        /// Returns the Current Max id in the given Table
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int GetMaxId(string databaseName, string sqlQuery)
        {
            EstablishSqlConnection(databaseName);
            int maxId;
            
            using (SqlCommand dataCommand =
                    new SqlCommand(sqlQuery, _sqlConnection))
            {

                maxId = Convert.ToInt32(dataCommand.ExecuteScalar());
            }

            CloseSqlConnection();
            return maxId;
        }
        #endregion

        [Test]
        public void ListTablesHasData()
        {
            var count = 0;
            foreach (var database in Databases)
            {
                ConnectToDatabase(database);
                var schema = GetDatabaseSchema();
                var tables = GetTables(schema);

                foreach (var table in tables)
                {
                    EstablishSqlConnection(database);
                    var counts = 0;

                    using (SqlCommand dataCommand =
                            new SqlCommand("select count(*) from " + table, _sqlConnection))
                    {

                        counts = Convert.ToInt32(dataCommand.ExecuteScalar());
                    }

                    CloseSqlConnection();

                    if (counts > 0)
                    {
                        Console.WriteLine(database + "==>" + table);
                    }
                }
                DisConnectFromDatabase();
            }
            Console.WriteLine("Total Tables: " + count);
            
        }

        [Test]
        public void ListTablesHasCustomerColumns()
        {
            var count = 0;
            foreach (var database in Databases)
            {
                ConnectToDatabase(database);
                var schema = GetDatabaseSchema();
                var tables = GetTables(schema);

                foreach (var table in tables)
                {
                    var tblSchema = GetTableColumns(table);

                    for (int i = 0; i < tblSchema.Rows.Count; i++)
                    {
                        var column = tblSchema.Rows[i].ItemArray[3].ToString();


                        if (column.Equals("ApplicationId") || column.Equals("AccountId"))
                        {
                            count++;
                            Console.WriteLine(database + "==>" + table + "==>" + column);
                            break;
                        }
                    }
                }
                DisConnectFromDatabase();
            }
            Console.WriteLine("Total Tables: " + count);

        }

        [Test]
        public void BackUpDatabases()
        {
            EstablishServerConnection();
            foreach (var database in Databases)
            {
                DoBackupDatabase(database, database + BackupExtension);
            }
            TerminateServerConnection();
        }

        [Test]
        public void RestoreDatabases()
        {
            var wmi = new WmiUtil();
            var scope = wmi.EstablishConnection(AppServer, Username, Password, ManagementPath);

            wmi.StopAllWongaServices(scope);
            wmi.StopService(scope, "MSSQLSERVER");
            System.Threading.Thread.Sleep(10000);
            wmi.StartService(scope, "MSSQLSERVER");
            System.Threading.Thread.Sleep(20000);

            EstablishServerConnection();
            foreach (var database in Databases)
            {
                Console.WriteLine("Restoring database " + database);
                DoRestoreDatabase(database, database + BackupExtension);
                Console.WriteLine("Restore Database completed successfully for " + database);
            }
            TerminateServerConnection();

            wmi.StartAllWongaServices(scope);
        }
    }
}
