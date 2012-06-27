using System;
using System.Xml;

using Microsoft.SqlServer.Management.Smo;

namespace Wonga.QA.Framework.Data
{
    public class SMODatabaseAlterations
    {

        /// <summary>
        /// To add a column to a table
        /// </summary>
        /// <param name="serverName">The name of the server to connect to</param>
        /// <param name="databaseName">The name of the database to add the column to</param>
        /// <param name="schemaName">The schema name of the table</param>
        /// <param name="tableName">The table name to add the column</param>
        /// <param name="columnToAdd">The name of the column to add</param>
        /// <param name="dataTypeToUse">The data type of the column to add</param>
        public static void AddAColumn(string serverName, string databaseName, string schemaName, string tableName, string columnToAdd, DataType dataTypeToUse)
        {
            try
            {

                Server srv = new Server(serverName);
                Database db = srv.Databases[databaseName];
                db.DefaultSchema = schemaName;

                Table tbl = db.Tables[tableName];

                Column col = new Column(tbl, columnToAdd, dataTypeToUse);
                tbl.Columns.Add(col);
                tbl.Alter();

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// To remove a column from a table
        /// </summary>
        /// <param name="serverName">The name of the server to connect to</param>
        /// <param name="databaseName">The name of the database to remove the column from</param>
        /// <param name="schemaName">The schema name of the table</param>
        /// <param name="tableName">The table name to remove the column from</param>
        /// <param name="columnToRemove">The name of the column to remove</param>
        public static void RemoveAColumn(string serverName, string databaseName, string schemaName, string tableName, string columnToRemove)
        {
            try
            {
                Server srv = new Server(serverName);
                Database db = srv.Databases[databaseName];
                db.DefaultSchema = schemaName;
                if (db.Tables.Contains(tableName))
                {
                    Table tbl = db.Tables[tableName];
                    if (tbl.Columns.Contains(columnToRemove))
                    {
                        tbl.Columns[columnToRemove].Drop();
                        tbl.Alter();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
