namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    class Draw
    {
        public const char BorderCharacterVertical = '\u2588';     // vertical border character
        public const char BorderCharacterHorizontal = '\u2588';   // horizontal border character

        // Draw intro Logo
        public static void DrawLogo()
        {
            string[,] logo = new string[34, 1];
            int rowCounter = 4; // used for setting an initial cursor position for printing the labyrinth

            // Reading the map from external txt file
            using (StreamReader logoFile = new StreamReader(@"..\..\logo\logo.txt"))
            {
                // Filling the 2D string array
                for (int row = 0; row < logo.GetLength(0); row++)
                {
                    for (int col = 0; col < logo.GetLength(1); col++)
                    {
                        logo[row, col] = logoFile.ReadLine();
                    }
                }
            }

            // Print the logo
            for (int row = 0; row < logo.GetLength(0); row++)
            {
                for (int col = 0; col < logo.GetLength(1); col++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Questions.Print(rowCounter, 12, logo[row, col]);
                }
                rowCounter++;
            }

            string developedBy = "DEVELOPED BY: TEAM PSYCHIC ELEMENTAL";
            string telerikAcademy = "TELERIK ACADEMY";

            int startposition = Questions.GameWidth / 2 - (developedBy.Length - 1) / 2;
            int startposition1 = Questions.GameWidth / 2 - (telerikAcademy.Length - 1) / 2;

            Questions.Print(Console.WindowHeight - 2, startposition, developedBy);
            Questions.Print(Console.WindowHeight - 1, startposition1, telerikAcademy);
            Console.ReadLine();
        }

        // Draw labyrinth
        public static void DrawLabyrinth()
        {
            string[,] map = new string[36, 1];
            int rowCounter = 3; // used for setting an initial cursor position for printing the labyrinth

            // Reading the map from external txt file
            using (StreamReader labyrinth = new StreamReader(@"..\..\labyrinth\map3.txt"))
            {
                // Filling the 2D string array
                for (int row = 0; row < map.GetLength(0); row++)
                {
                    for (int col = 0; col < map.GetLength(1); col++)
                    {
                        map[row, col] = labyrinth.ReadLine();
                    }
                }
            }

            // Print the map
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Questions.Print(rowCounter, Questions.FieldWidth + 6, map[row, col]);
                }
                rowCounter++;
            }
        }

        // Draw screen menu
        public static void DrawMenuScreen()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            // Drawing first and last row border
            for (int col = 0; col < Questions.GameWidth; col++)
            {
                Questions.Print(0, col, BorderCharacterVertical); // Top Border
                //Print();
                Questions.Print(Questions.GameHeight - 1, col, BorderCharacterVertical);  // Bottom border
            }

            // Drawing Vertical borders
            for (int row = 0; row < Questions.GameHeight; row++)
            {
                Questions.Print(row, 0, BorderCharacterVertical); // Left border

                Questions.Print(row, Questions.FieldWidth + 1 + Questions.InfoPanelWidth + 1, BorderCharacterVertical);  // Right border
            }
        }

        // Draw borders
        public static void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            // Drawing first and last row border
            for (int col = 0; col < Questions.GameWidth; col++)
            {
                Questions.Print(0, col, BorderCharacterHorizontal); // Top Border
                //Print();
                Questions.Print(Questions.GameHeight - 1, col, BorderCharacterHorizontal);  // Bottom border
            }

            // Drawing Vertical borders
            for (int row = 0; row < Questions.GameHeight; row++)
            {
                Questions.Print(row, 0, BorderCharacterVertical); // Left border
                Questions.Print(row, Questions.FieldWidth + 1, BorderCharacterVertical);   // Middle vertical line
                Questions.Print(row, Questions.FieldWidth + 1 + Questions.InfoPanelWidth + 1, BorderCharacterVertical);  // Right border
            }
        }

        // Print Credits
        public static void PrintCredits()
        {
            Draw.DrawMenuScreen();

            Console.ForegroundColor = ConsoleColor.Green;

            string credits = "CREDITS:";
            string telerikAcademy = "TELERIK ACADEMY";
            string finalLine = String.Empty;

            int startposition = Questions.GameWidth / 2 - (credits.Length - 1) / 2;
            int startposition1 = Questions.GameWidth / 2 - (telerikAcademy.Length - 1) / 2;

            Questions.Print(5, startposition, credits);
            Questions.Print(7, startposition1, telerikAcademy);
            Questions.Print(9, startposition1 - 5, "TEAM: PSYCHIC ELEMENTAL");
            Questions.Print(11, startposition1 - 5, "TEAM MEMBERS:");

            Questions.Print(13, startposition1 - 5, "ABELINA GEORGIEVA");
            Questions.Print(13, startposition1 + 25, "(abelina)");

            Questions.Print(14, startposition1 - 5, "BOZHKO BOZHKOV");
            Questions.Print(14, startposition1 + 25, "(bbojkov)");

            Questions.Print(15, startposition1 - 5, "DIANA IVANOVA");
            Questions.Print(15, startposition1 + 25, "(diana.ivanova)");

            Questions.Print(16, startposition1 - 5, "KONSTANTIN ISKROV");
            Questions.Print(16, startposition1 + 25, "(iskroff)");

            Questions.Print(17, startposition1 - 5, "PETAR ALEXANDROV");
            Questions.Print(17, startposition1 + 25, "(P.Alexandrov)");

            Questions.Print(18, startposition1 - 5, "PETAR PETROV");
            Questions.Print(18, startposition1 + 25, "(eudaimonia)");

            Questions.Print(19, startposition1 - 5, "SVETOSLAV IVANOV");
            Questions.Print(19, startposition1 + 25, "(Inxslackware)");

            Questions.Print(20, startposition1 - 5, "VYARA HRISTOVA");
            Questions.Print(20, startposition1 + 25, "(vyarah)");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Questions.Print(Console.WindowHeight - 2, 3, finalLine);

        }

        // Anouncing the winner and GAME OVER!
        public static void GameOver()
        {
            //DrawMenuScreen();

            string announceWinner = "THE WINNER IS:";
            string gameOver = "GAME OVER !";
            int startposition = Questions.GameWidth / 2 - ((announceWinner.Length - 1) / 2) + Questions.InfoPanelWidth / 2 + 1;
            int startposition1 = Questions.GameWidth / 2 - (Questions.p1Input.Length - 1) / 2 + Questions.InfoPanelWidth / 2 + 1;
            int startposition2 = Questions.GameWidth / 2 - (Questions.p2Input.Length - 1) / 2 + Questions.InfoPanelWidth / 2 + 1;
            int startposition3 = Questions.GameWidth / 2 - (gameOver.Length - 1) / 2 + Questions.InfoPanelWidth / 2 + 1;

            Console.ForegroundColor = ConsoleColor.Green;
            Questions.Print(10, startposition, announceWinner);

            // Announce player 1 as winner 
            if (Movement.winnerP1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Questions.Print(12, startposition1, Questions.p1Input);
            }// Announce player 2 is winner
            else if (Movement.winnerP2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Questions.Print(12, startposition2, Questions.p2Input);
            }

            // Print GAME OVER!
            Console.ForegroundColor = ConsoleColor.Green;
            Questions.Print(14, startposition3, gameOver);
        }

        // Print players info
        public static void PrintPlayerInfo()
        {

            string player1Label = "PLAYER 1:";
            string player2Label = "PLAYER 2:";

            Console.ForegroundColor = ConsoleColor.Green;
            Questions.Print(3, 8, player1Label);
            Questions.Print(3, 45, player2Label);

            Console.ForegroundColor = ConsoleColor.White;
            Questions.Print(4, 8, Questions.p1Input);

            Console.ForegroundColor = ConsoleColor.Red;
            Questions.Print(4, 45, Questions.p2Input);
        }

        public static void PrintAnswersToWin()
        {
            //change if both players are at the final
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int playerOneAnswersLeft = Movement.p1MovementCoords.GetLength(0) - Movement.p1Move;
            int playerTwoAnswersLeft = Movement.p1MovementCoords.GetLength(0) - Movement.p2Move;
            Questions.Print(Console.WindowHeight - 2, 3, string.Format("Player 1 - {0} correct answers to win", playerOneAnswersLeft));
            Questions.Print(Console.WindowHeight - 1, 3, string.Format("Player 2 - {0} correct answers to win", playerTwoAnswersLeft));
            Console.ResetColor();
        }

        // Print game menu
        public static void PrintMenu()
        {
            string menuTitle = "MENU";
            string player1Name = "PLAYER 1: ENTER NAME";
            string player2Name = "PLAYER 2: ENTER NAME";

            int startposition = Questions.GameWidth / 2 - (menuTitle.Length - 1) / 2;

            int startposition1 = Questions.GameWidth / 2 - 11;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Questions.Print(8, startposition, menuTitle);

            Console.ForegroundColor = ConsoleColor.Green;
            Questions.Print(10, startposition1, player1Name);
            Questions.Print(11, startposition1 - 1, ' ');

            Console.ForegroundColor = ConsoleColor.White;
            Questions.p1Input = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Questions.Print(13, startposition1, player2Name);
            Questions.Print(14, startposition1 - 1, ' ');

            Console.ForegroundColor = ConsoleColor.Red;
            Questions.p2Input = Console.ReadLine();
        }
    }
}