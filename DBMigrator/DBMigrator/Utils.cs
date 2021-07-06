using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DBMigrator
{
    internal static class Utils
    {
        /// <summary>
        /// Get args and build connection string
        /// </summary>
        /// <param name="args">Command line args</param>
        ///<param name="initDb">Flag for signalling to connect to the database server instance and not a particular database in the instance</param>
        /// <returns>The database connection string</returns>
        public static string GetConnectionString(string[] args, bool initDb = false)
        {
            string connString = ConfigurationManager
                    .ConnectionStrings[0]
                    .ConnectionString;

            connString = initDb ? connString.Replace("Database=[DATABASE];", "") : connString;

            if (args.Length > 4)
            {
                connString = connString.Replace("[SERVER]", args[0])
                    .Replace("[USER]", args[2])
                    .Replace("[PASSWORD]", args[3]);

                connString = !initDb ? connString.Replace("[DATABASE]", args[1]) : connString;
            }

            return connString;
        }

        public static async Task CreateDBAsync(string connectionString, string databaseName)
        {
            try
            {
                await ExecuteNonQueryAsync(connectionString, CreateDatabaseScript(databaseName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task DropDBAsync(string connectionString, string databaseName)
        {
            try
            {
                await ExecuteNonQueryAsync(connectionString, DropDatabaseScript(databaseName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        private static string DropDatabaseScript(string databaseName)
        {
            string sql = $@"
                IF EXISTS (
                    SELECT *
                    FROM sys.databases
                    WHERE name = '{databaseName}'
                    )
                    BEGIN
	                    ALTER DATABASE {databaseName} set single_user with rollback immediate
                        DROP Database {databaseName}
                    END
                ";

            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        private static string CreateDatabaseScript(string databaseName)
        {
            string sql = $@"
                IF NOT EXISTS (
                    SELECT *
                    FROM sys.databases
                    WHERE name = '{databaseName}'
                    )
                    BEGIN
	                    CREATE Database {databaseName}
                    END
            ";

            return sql;
        }

        private static async Task<int> ExecuteNonQueryAsync(string connectionString, string sqlStatement)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = (SqlCommand)SqlClientFactory.Instance.CreateCommand())
                {
                    cmd.CommandText = sqlStatement;
                    cmd.Connection = conn;
                    await conn.OpenAsync();
                    rowsAffected = await cmd.ExecuteNonQueryAsync();
                    await conn.CloseAsync();
                }
            }

            return rowsAffected;
        }
    }
}
