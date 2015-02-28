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

        const char BorderCharacter = (char)219;     // Border character

        static string p1Input;
        static string p2Input;

        static void Main()
        {
            // Setting Game Title
            Console.Title = "C# Scramble";

            // Set Encoding
            Console.OutputEncoding = Encoding.GetEncoding(1252);

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

            GenerateQuestion();            

            //Added Timer
            TimerCallback callback = new TimerCallback(Tick);
            Timer stateTimer = new Timer(callback, null, 0, 1000);
            //waiting for the user to press any button
            Console.ReadLine();

        }

        static void DrawLabyrinth()
        {
            string[,] map = new string[36, 1];
            int rowCounter = 3; // used for setting an initial cursor position for printing the labyrinth

            // Reading the map from external txt file
            using (StreamReader labyrinth = new StreamReader(@"..\..\labyrinth\map.txt"))
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Print(rowCounter, FieldWidth + 17, map[row, col]);
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

        static void GenerateQuestion()
        {
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


            Print(8, 5, question1.text);
            Print(10, 9, question1.a);
            Print(11, 9, question1.b);
            Print(12, 9, question1.c);
            Print(13, 9, question1.d);
            
            Print(15, 5, "Choose an answer...");

            if (IsAnsweredCorrect(question1.correctAnswer))
            {
                Print(17, 5, "Correct!");
                Console.WriteLine();
            }
            else
            {
                Print(17, 5, "Incorrect!");
                Console.WriteLine();
            }
        }

        static void DrawMenuScreen()
        {
            // Drawing first and last row border
            for (int col = 0; col < GameWidth; col++)
            {
                Print(0, col, BorderCharacter); // Top Border
                //Print();
                Print(GameHeight - 1, col, BorderCharacter);  // Bottom border
            }

            // Drawing Vertical borders
            for (int row = 0; row < GameHeight; row++)
            {
                Print(row, 0, BorderCharacter); // Left border

                Print(row, FieldWidth + 1 + InfoPanelWidth + 1, BorderCharacter);  // Right border
            }

            string menuTitle = "MENU";
            string player1Name = "PLAYER 1: ENTER NAME";
            string player2Name = "PLAYER 2: ENTER NAME";

            //string p2Input = Console.ReadLine();
            int startposition = GameWidth / 2 - (menuTitle.Length - 1) / 2;
            //startposition = startposition + FieldWidth + 2;

            int startposition1 = GameWidth / 2 - 11;

            Print(8, startposition, menuTitle);

            Print(10, startposition1, player1Name);
            Print(11, startposition1 - 1, ' ');

            p1Input = Console.ReadLine();

            Print(13, startposition1, player2Name);
            Print(14, startposition1 - 1, ' ');
            p2Input = Console.ReadLine();

        }

        static void DrawBorders()
        {
            // Drawing first and last row border
            for (int col = 0; col < GameWidth; col++)
            {
                Print(0, col, BorderCharacter); // Top Border
                //Print();
                Print(GameHeight - 1, col, BorderCharacter);  // Bottom border
            }

            // Drawing Vertical borders
            for (int row = 0; row < GameHeight; row++)
            {
                Print(row, 0, BorderCharacter); // Left border
                Print(row, FieldWidth + 1, BorderCharacter);   // Middle vertical line
                Print(row, FieldWidth + 1 + InfoPanelWidth + 1, BorderCharacter);  // Right border
            }
        }

        static void Print(int row, int col, object data)
        {
            // Usually first receives Column, then Row, here we do the oposite
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(col, row);
            Console.Write(data);
        }

        static void Tick(Object stateInfo)
        {
            Console.Write("\rTick Tack: {0}", DateTime.Now.ToString("h:mm:ss"));            
        }
    }
}
