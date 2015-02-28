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

        static string p1Input;
        static string p2Input;

        static int questionCounter = 0; // question counter

        static void Main()
        {
            // Setting Game Title
            Console.Title = "C# Scramble";

            // Set Encoding
            Console.OutputEncoding = Encoding.UTF8;

            //Console.CursorVisible = false;

            // Removing unusable space
            Console.WindowWidth = GameWidth;
            Console.BufferWidth = GameWidth;
            Console.WindowHeight = GameHeight + 2;
            Console.BufferHeight = GameHeight + 2;

            // Draw menu
            DrawMenuScreen();

            Console.Clear();

            // Draw game field
            DrawBorders();

            // Draw Labyrinth
            DrawLabyrinth();
            PrintPlayerInfo();

            GenerateQuestion();

            //Added Timer
            TimerCallback callback = new TimerCallback(Tick);
            Timer stateTimer = new Timer(callback, null, 0, 1000);
            //waiting for the user to press any button
            Console.ReadLine();
        }

        static void PrintPlayerInfo()
        {
            string infoFormatter1 = "{0,-22}VS{1,22}";
            string player1Label = "PLAYER 1:";
            string player2Label = "PLAYER 2:";

            Console.ForegroundColor = ConsoleColor.Green;
            Print(3, 8, String.Format(infoFormatter1, player1Label, player2Label));

            Console.ForegroundColor = ConsoleColor.White;
            Print(4, 8, p1Input);
            Print(4, 45, p2Input);
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

            if (pressedKey.KeyChar == key)
            {
                return true;
            }

            return false;
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

            Console.ForegroundColor = ConsoleColor.Red;

            Print(7, 5, "Question: " + questionCounter);

            Console.ForegroundColor = ConsoleColor.Green;
            Print(9, 5, question1.text);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Print(11, 9, question1.a);
            Print(12, 9, question1.b);
            Print(13, 9, question1.c);
            Print(14, 9, question1.d);

            Console.ForegroundColor = ConsoleColor.Yellow;

            Print(16, 5, "Choose an answer .. ");

            if (IsAnsweredCorrect(question1.correctAnswer))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Print(17, 5, "Correct!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Print(17, 5, "Incorrect!");
            }
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

            Console.ForegroundColor = ConsoleColor.White;
            p2Input = Console.ReadLine();

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

        static void Tick(Object stateInfo)
        {
            Console.Write("\rTick Tack: {0}", DateTime.Now.ToString("h:mm:ss"));
        }
    }
}
