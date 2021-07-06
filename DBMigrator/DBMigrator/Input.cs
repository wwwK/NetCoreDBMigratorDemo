using System;
using System.Collections.Generic;
using System.Text;

namespace DBMigrator
{
    internal class Input
    {
        /// <summary>
        /// Print Line
        /// </summary>
        /// <param name="msg">Message to print to console</param>
        public static void P(string msg = null)
        {
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Get Type from console
        /// </summary>
        /// <typeparam name="T">The data type expected</typeparam>
        /// <param name="msg">Message to show the user</param>
        /// <param name="inLine">Places the user input on the same line if set to true</param>
        /// /// <param name="inputColour">Set colour displayed for user input. Defaults to white</param>
        /// <returns>return the next T</returns>
        public static T Get<T>(string msg, bool inLine = false, ConsoleColor inputColour = ConsoleColor.White)
        {
            T returnValue = default(T);
            bool isValid = false;

            if (!inLine) Console.WriteLine(msg); else Console.Write(msg);


            while (!isValid)
            {
                try
                {
                    Console.ForegroundColor = inputColour;
                    returnValue = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                    isValid = true;
                }
                catch (Exception)
                {
                    isValid = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Maskes input of text within a Console and returns the real input value from the user.
        /// Useful for masking passwords
        /// </summary>
        /// <param name="maskCharacter">The masking character. Default "*"</param>
        /// <returns>The content of the input</returns>
        public static string Masked(string maskCharacter = "*")
        {
            string hideText = "";

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    hideText += key.KeyChar;
                    Console.Write(maskCharacter);
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && hideText.Length > 0)
                    {
                        hideText = hideText.Substring(0, (hideText.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            return hideText;
        }
    }
}
