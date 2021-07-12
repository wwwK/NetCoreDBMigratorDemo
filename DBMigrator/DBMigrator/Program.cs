using DbUp;
using DbUp.Engine;
using System;
using System.Reflection;
using System.Threading.Tasks;
using I = DBMigrator.Input;

namespace DBMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            bool automation = args.Length != 0;
            I.P("DB Migrator Demo");
            I.P(await ExecuteDBUpAsync(await ProcessInputArgsAsync(args)) == 1 ? "Ok" : "Failed");

            Console.ForegroundColor = ConsoleColor.White;

            if (!automation)
            {
                I.P("Press any key to end...");
                Console.ReadLine();
            }
        }

        private async static Task<string[]> ProcessInputArgsAsync(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[5];
                Console.ForegroundColor = ConsoleColor.Green;
                args[0] = I.Get<string>("Provide a server: ", true, ConsoleColor.Yellow);
                Console.ForegroundColor = ConsoleColor.Green;
                args[1] = I.Get<string>("Provide a database: ", true, ConsoleColor.Yellow);
                Console.ForegroundColor = ConsoleColor.Green;
                args[2] = I.Get<string>("User: ", true, ConsoleColor.Yellow);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Password: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                args[3] = I.Masked();
                Console.ForegroundColor = ConsoleColor.Green;
                I.P();
                args[4] = I.Get<string>("Drop database? y/n: ", true, ConsoleColor.Yellow) == "y" ? "drop" : "";
            }
            else
            {
                I.P("Args provided. Running DB Migration....");
            }

            if (args.Length == 5 && args[4] == "drop") await Utils.DropDBAsync(Utils.GetConnectionString(args,true), args[1]);

            return args;
        }

        private static async Task<int> ExecuteDBUpAsync(string[] args)
        {
            int returnDatabases = 0;

            try
            {
                I.P($"Connection string built for Server: {args[0]} Database: {args[1]}.");

                //Gen DB
                await Utils.CreateDBAsync(Utils.GetConnectionString(args,true), args[1]);
                
                UpgradeEngine upgrader = DeployChanges.To
                            .SqlDatabase(Utils.GetConnectionString(args))
                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                            .LogToConsole()
                            .WithExecutionTimeout(TimeSpan.FromSeconds(999999))
                            .Build();

                I.P("Starting migration....");

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    I.P($"Migration error successfully Server: { args[0]} Database: { args[1]}");
                    I.P(result.Error.ToString());
                }
                else
                {
                    I.P($"Migration complete successfully Server: { args[0]} Database: { args[1]}");
                    returnDatabases++;
                }                    
            }
            catch (Exception ex)
            {
                I.P(ex.Message);
            }

            return returnDatabases;
        }
    }
}
