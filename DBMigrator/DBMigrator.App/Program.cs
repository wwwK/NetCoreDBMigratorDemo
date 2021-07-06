using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DBMigrator.App
{
    class Program
    {
        static string connectionString = string.Empty;

        static void Main(string[] args)
        {
            connectionString =
            args.FirstOrDefault()
            ?? "Server=mga.local;Database=MyApp;uid=dev;password=dev1234";

            try
            {
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to quit");
            Console.ReadLine();
        }

        public static async Task<DataSet> ExecuteQueryAsync(string sqlStatement, CommandType type = CommandType.Text, SqlParameter[] prms = null)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = (SqlDataAdapter)SqlClientFactory.Instance.CreateDataAdapter();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = (SqlCommand)SqlClientFactory.Instance.CreateCommand())
                {
                    if (prms != null)
                        cmd.Parameters.AddRange(prms.Where(p => p != null).ToArray());

                    cmd.CommandType = type;
                    cmd.CommandText = sqlStatement;
                    da.SelectCommand = (SqlCommand)cmd;
                    da.SelectCommand.Connection = conn;

                    await Task.Run(() => da.Fill(ds));
                }
            }

            return ds;
        }
    }
}
