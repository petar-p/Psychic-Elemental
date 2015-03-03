using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class PlayerNameValidator
    {
        public static string ReadValidName(int y, int x)
        {
            string playerName = string.Empty;
            do
            {
                try
                {
                    string readedName = Console.ReadLine();

                    if (string.IsNullOrEmpty(readedName))
                    {
                        throw new ArgumentException("Name must not be empty.");
                    }

                    playerName = readedName;
                }
                catch (ArgumentException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Questions.Print(y, x, ex.Message);
                    Console.SetCursorPosition(x - 21, y + 1);
                    Console.ResetColor();
                }
                
            } while (!IsValidName(playerName));

            return playerName;
        }

        private static bool IsValidName(string playerName)
        {
            return !string.IsNullOrEmpty(playerName);
        }
    }
}
