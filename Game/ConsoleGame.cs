namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.IO;

    class ConsoleGame
    {
        struct Question
        {
            public string text;
            public string a;
            public string b;
            public string c;
            public string d;
            public char correctAnswer;
        }

        const int GameWidth = FieldWidth + InfoPanelWidth + 3;
        const int GameHeight = FieldHeigth + 2;
        const int FieldWidth = 60;
        const int FieldHeigth = 40;
        const int InfoPanelWidth = 60;  // Info panel on the right

        const char BorderCharacterVertical = '\u2588';     // vertical border character
        const char BorderCharacterHorizontal = '\u2588';   // horizontal border character

        static char player1Character = '\u25A0';    //u263A
        static char player2Character = '\u25A0';    //u263B

        static string p1Input;
        static string p2Input;

        static bool p1Answer = false;
        static bool p2Answer = false;
        static bool isThereAWinner = false;
        static bool winnerP1 = false;
        static bool winnerP2 = false;

        static int questionCounter = 0; // question counter
        static int p1Move = 0;
        static int p2Move = 0;

        // Coordinates of Player 1 moving positions on the map
        static int[,] p1MovementCoords = new int[,] 
        {
            {38, 90},
            {33, 90},
            {33, 78},
            {29, 78},
            {29, 70},
            {24, 70},
            {24, 74},
            {17, 74},
            {17, 70},
            {12, 70},
            {12, 78},
            { 8, 78},
            { 8, 90},
            { 3, 90}
        };

        // Coordinates of Player 2 moving positions on the map
        static int[,] p2MovementCoords = new int[,] 
        {
            {38, 94},
            {33, 94},
            {33, 106},
            {29, 106},
            {29, 114},
            {24, 114},
            {24, 110},
            {17, 110},
            {17, 114},
            {12, 114},
            {12, 106},
            { 8, 106},
            { 8, 94},
            { 3, 94}
        };

        static void Main()
        {
            // Setting Game Title
            Console.Title = "C# Scramble";

            // Set Encoding
            Console.OutputEncoding = Encoding.UTF8;

            // Removing unusable space
            Console.WindowWidth = GameWidth;
            Console.BufferWidth = GameWidth;
            Console.WindowHeight = GameHeight + 2;
            Console.BufferHeight = GameHeight + 2;

            DrawMenuScreen();
            DrawLogo();
            Console.Clear();

            // Draw menu
            DrawMenuScreen();
            PrintMenu();

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Print(38, 90, player1Character);
            Console.ForegroundColor = ConsoleColor.Red;
            Print(38, 94, player2Character);

            while (true)
            {
                Console.CursorVisible = false;

                // Draw game field
                DrawBorders();

                PrintPlayerInfo();

                // Draw Labyrinth
                DrawLabyrinth();

                PrintPlayersNextPosition();

                PrintAnswersToWin();

                // Check for winner
                if (p1Move == 13 || p2Move == 13)
                {
                    if (CheckForWinner())
                    {
                        break;  // GAME OVER ! There's a winner
                    }
                    else
                    {
                        // Both players are at the end of the labyrinth, that means equal score

                        // TODO: Decide how to name the winner... penalties or something else
                        continue;
                    }
                }
                
                GenerateQuestion();

                Player1Movement();
                Player2Movement();

                Console.Clear();
            }

            GameOver();
            Console.ReadLine();
            Console.Clear();
            PrintCredits();
            Console.ReadLine();

        }

        // Print Credits
        static void PrintCredits()
        {
            DrawMenuScreen();

            Console.ForegroundColor = ConsoleColor.Green;

            string credits = "CREDITS:";
            string telerikAcademy = "TELERIK ACADEMY";
            string finalLine = String.Empty;

            int startposition = GameWidth / 2 - (credits.Length - 1) / 2;
            int startposition1 = GameWidth / 2 - (telerikAcademy.Length - 1) / 2;

            Print(5, startposition, credits);
            Print(7, startposition1, telerikAcademy);
            Print(9, startposition1 - 5, "TEAM: PSYCHIC ELEMENTAL");
            Print(11, startposition1 - 5, "TEAM MEMBERS:");

            Print(13, startposition1 - 5, "ABELINA GEORGIEVA");
            Print(13, startposition1 + 25, "(abelina)");

            Print(14, startposition1 - 5, "BOZHKO BOZHKOV");
            Print(14, startposition1 + 25, "(bbojkov)");

            Print(15, startposition1 - 5, "DIANA IVANOVA");
            Print(15, startposition1 + 25, "(diana.ivanova)");

            Print(16, startposition1 - 5, "KONSTANTIN ISKROV");
            Print(16, startposition1 + 25, "(iskroff)");

            Print(17, startposition1 - 5, "PETAR ALEXANDROV");
            Print(17, startposition1 + 25, "(P.Alexandrov)");

            Print(18, startposition1 - 5, "PETAR PETROV");
            Print(18, startposition1 + 25, "(eudaimonia)");

            Print(19, startposition1 - 5, "SVETOSLAV IVANOV");
            Print(19, startposition1 + 25, "(Inxslackware)");

            Print(20, startposition1 - 5, "VYARA HRISTOVA");
            Print(20, startposition1 + 25, "(vyarah)");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Print(Console.WindowHeight - 2, 3, finalLine);

        }

        // Draw intro Logo
        static void DrawLogo()
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
                    Print(rowCounter, 12, logo[row, col]);
                }
                rowCounter++;
            }

            string developedBy = "DEVELOPED BY: TEAM PSYCHIC ELEMENTAL";
            string telerikAcademy = "TELERIK ACADEMY";

            int startposition = GameWidth / 2 - (developedBy.Length - 1) / 2;
            int startposition1 = GameWidth / 2 - (telerikAcademy.Length - 1) / 2;

            Print(Console.WindowHeight - 2, startposition, developedBy);
            Print(Console.WindowHeight - 1, startposition1, telerikAcademy);
            Console.ReadLine();
        }

        // Print game menu
        static void PrintMenu()
        {
            string menuTitle = "MENU";
            string player1Name = "PLAYER 1: ENTER NAME";
            string player2Name = "PLAYER 2: ENTER NAME";

            int startposition = GameWidth / 2 - (menuTitle.Length - 1) / 2;

            int startposition1 = GameWidth / 2 - 11;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Print(8, startposition, menuTitle);

            Console.ForegroundColor = ConsoleColor.Green;
            Print(10, startposition1, player1Name);
            Print(11, startposition1 - 1, ' ');

            Console.ForegroundColor = ConsoleColor.White;
            p1Input = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Print(13, startposition1, player2Name);
            Print(14, startposition1 - 1, ' ');

            Console.ForegroundColor = ConsoleColor.Red;
            p2Input = Console.ReadLine();
        }

        // Anouncing the winner and GAME OVER!
        static void GameOver()
        {
            //DrawMenuScreen();

            string announceWinner = "THE WINNER IS:";
            string gameOver = "GAME OVER !";
            int startposition = GameWidth / 2 - ((announceWinner.Length - 1) / 2) + InfoPanelWidth / 2 + 1;
            int startposition1 = GameWidth / 2 - (p1Input.Length - 1) / 2 + InfoPanelWidth / 2 + 1;
            int startposition2 = GameWidth / 2 - (p2Input.Length - 1) / 2 + InfoPanelWidth / 2 + 1;
            int startposition3 = GameWidth / 2 - (gameOver.Length - 1) / 2 + InfoPanelWidth / 2 + 1;

            Console.ForegroundColor = ConsoleColor.Green;
            Print(10, startposition, announceWinner);

            // Announce player 1 as winner 
            if (winnerP1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Print(12, startposition1, p1Input);
            }// Announce player 2 is winner
            else if (winnerP2)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Print(12, startposition2, p2Input);
            }

            // Print GAME OVER!
            Console.ForegroundColor = ConsoleColor.Green;
            Print(14, startposition3, gameOver);
        }

        // Check for winner
        static bool CheckForWinner()
        {
            if (p1Move == 13 && p2Move != 13)
            {
                // GAME OVER! The winner in Player 1
                winnerP1 = true;    // flag that Player 1 is winner
                isThereAWinner = true;
            }
            else if (p2Move == 13 && p1Move != 13)
            {
                // GAME OVER! The winner in Player 2
                winnerP2 = true;    // flag that Player 2 is winner
                isThereAWinner = true;
            }
            else if (p1Move == 13 && p2Move == 13)
            {
                // Both players are at the end of the labyrinth, that means equal score

                // TODO: Decide how to name the winner... penalties or something else
                isThereAWinner = false;
            }

            return isThereAWinner;
        }

        // Print players next positions
        static void PrintPlayersNextPosition()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Print(p1MovementCoords[p1Move, 0], p1MovementCoords[p1Move, 1], player1Character);

            Console.ForegroundColor = ConsoleColor.Red;
            Print(p2MovementCoords[p2Move, 0], p2MovementCoords[p2Move, 1], player2Character);
        }

        static void Player1Movement()
        {
            if (p1Answer)
            {
                p1Move++;
            }
            else
            {
                p1Move--;
                if (p1Move < 0)
                {
                    p1Move = 0;
                }
            }
        }

        static void Player2Movement()
        {
            if (p2Answer)
            {
                p2Move++;
            }
            else
            {
                p2Move--;
                if (p2Move < 0)
                {
                    p2Move = 0;
                }
            }
        }

        // Print players info
        static void PrintPlayerInfo()
        {

            string player1Label = "PLAYER 1:";
            string player2Label = "PLAYER 2:";

            Console.ForegroundColor = ConsoleColor.Green;
            Print(3, 8, player1Label);
            Print(3, 45, player2Label);

            Console.ForegroundColor = ConsoleColor.White;
            Print(4, 8, p1Input);

            Console.ForegroundColor = ConsoleColor.Red;
            Print(4, 45, p2Input);
        }

        static void PrintAnswersToWin()
        {
            //change if both players ar at the final
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int playerOneAnswersLeft = p1MovementCoords.GetLength(0) - p1Move;
            int playerTwoAnswersLeft = p1MovementCoords.GetLength(0) - p2Move;
            Print(Console.WindowHeight - 2, 3, string.Format("Player 1 - {0} correct answers to win", playerOneAnswersLeft));
            Print(Console.WindowHeight - 1, 3, string.Format("Player 2 - {0} correct answers to win", playerTwoAnswersLeft));
            Console.ResetColor();
        }

        // Draw labyrinth
        static void DrawLabyrinth()
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
                    Print(rowCounter, FieldWidth + 6, map[row, col]);
                }
                rowCounter++;
            }
        }

        // Check if player answered correct
        static bool IsAnsweredCorrect(char key)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey();
            char playerChoice = char.ToLower(pressedKey.KeyChar);

            if (!IsValidAnswer(playerChoice))
            {
                throw new ArgumentException("Invalid input. You must choose a, b, c or d... ");
            }

            if (playerChoice == char.ToLower(key))
            {
                return true;
            }

            return false;
        }

        static bool IsValidAnswer(char inputKey)
        {
            switch (inputKey)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                    return true;
                default:
                    return false;
            }
        }

        // Generate question
        static void GenerateQuestion()
        {
            questionCounter++;

            //creates a list with starting points for all of the questions
            List<int> questionStartPositions = new List<int>();
            for (int i = 0; i < 198; i += 6)
            {
                questionStartPositions.Add(i);
            }
            //creates a new Question
            Question question1 = new Question();

            //randomizes starting positions
            Random randomGenerator = new Random();
            int rnd = randomGenerator.Next(questionStartPositions.Count);
            int position = questionStartPositions[rnd];

            //reads from the text file
            using (StreamReader nextquestion = new StreamReader(@"..\..\questions\questions.txt"))
            {
                //creates a list with all of the questions
                List<string> text = new List<string>();
                for (int i = 0; i < 198; i++)
                {
                    text.Add(nextquestion.ReadLine());
                }

                //assigns question properties using randomness
                question1.text = text[position];
                question1.a = text[position + 1];
                question1.b = text[position + 2];
                question1.c = text[position + 3];
                question1.d = text[position + 4];
                string correct = text[position + 5];
                question1.correctAnswer = correct[0];
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            Print(7, 5, "Question: " + questionCounter);

            Console.ForegroundColor = ConsoleColor.Green;
            PrintQuestion(9, 5, question1.text, FieldWidth - 5);

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintQuestion(11, 9, question1.a, FieldWidth - 10);
            PrintQuestion(13, 9, question1.b, FieldWidth - 10);
            PrintQuestion(15, 9, question1.c, FieldWidth - 10);
            PrintQuestion(17, 9, question1.d, FieldWidth - 10);

            // 

            Print(19, 9, question1.correctAnswer);

            Console.ForegroundColor = ConsoleColor.White;
            Print(22, 5, "Player 1, choose an answer .. ");
            var validPlayerOneInput = false;

            do
            {
                try
                {
                    p1Answer = IsAnsweredCorrect(question1.correctAnswer);
                    validPlayerOneInput = true;
                }
                catch (ArgumentException ex)
                {
                    Print(23, 5, ex.Message);
                    validPlayerOneInput = false;
                }

            } while (!validPlayerOneInput);

            Console.ForegroundColor = ConsoleColor.Red;
            Print(24, 5, "Player 2, choose an answer .. ");

            var validPlayerTwoInput = false;

            do
            {
                try
                {
                    p2Answer = IsAnsweredCorrect(question1.correctAnswer);
                    validPlayerTwoInput = true;
                }
                catch (ArgumentException ex)
                {
                    Print(25, 5, ex.Message);
                    validPlayerTwoInput = false;
                }

            } while (!validPlayerTwoInput);

            string correctAns = "The correct answer is: {0}";

            Console.ForegroundColor = ConsoleColor.Green;
            Print(26, 5, String.Format(correctAns, question1.correctAnswer));

            Console.ForegroundColor = ConsoleColor.Gray;
            Print(28, 5, "Next question ..");
            Console.ReadLine();
        }

        // Draw screen menu
        static void DrawMenuScreen()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            // Drawing first and last row border
            for (int col = 0; col < GameWidth; col++)
            {
                Print(0, col, BorderCharacterVertical); // Top Border
                //Print();
                Print(GameHeight - 1, col, BorderCharacterVertical);  // Bottom border
            }

            // Drawing Vertical borders
            for (int row = 0; row < GameHeight; row++)
            {
                Print(row, 0, BorderCharacterVertical); // Left border

                Print(row, FieldWidth + 1 + InfoPanelWidth + 1, BorderCharacterVertical);  // Right border
            }
        }

        // Draw borders
        static void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            // Drawing first and last row border
            for (int col = 0; col < GameWidth; col++)
            {
                Print(0, col, BorderCharacterHorizontal); // Top Border
                //Print();
                Print(GameHeight - 1, col, BorderCharacterHorizontal);  // Bottom border
            }

            // Drawing Vertical borders
            for (int row = 0; row < GameHeight; row++)
            {
                Print(row, 0, BorderCharacterVertical); // Left border
                Print(row, FieldWidth + 1, BorderCharacterVertical);   // Middle vertical line
                Print(row, FieldWidth + 1 + InfoPanelWidth + 1, BorderCharacterVertical);  // Right border
            }
        }

        // Printing on custom position
        static void Print(int row, int col, object data)
        {
            // Usually first receives Column, then Row, here we do the oposite
            Console.SetCursorPosition(col, row);
            Console.Write(data);
        }

        //printing questions
        static void PrintQuestion(int row, int col, string text, int maxTextWidth)
        {
            if (text.Length > maxTextWidth - 1)
            {

                //string[] splittedtext = text.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                int count = 1;
                while (text[maxTextWidth - count] != ' ')
                {
                    count++;
                }

                int middle = maxTextWidth - count;
                string firstHalf = text.Substring(0, middle);
                string secondHalf = text.Substring(middle);
                Console.SetCursorPosition(col, row);
                Console.Write(firstHalf);
                Console.SetCursorPosition(col, row + 1);
                Console.Write(secondHalf);
            }

            else
            {
                Console.SetCursorPosition(col, row);
                Console.Write(text);
            }
        }
    }
}
