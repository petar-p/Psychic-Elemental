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

            ////Added Timer
            //TimerCallback callback = new TimerCallback(Tick);
            //Timer stateTimer = new Timer(callback, null, 0, 1000);
            ////waiting for the user to press any button
            //Console.ReadLine();
        }

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
            int startposition = GameWidth / 2 - ((announceWinner.Length - 1) / 2) +  InfoPanelWidth/2 + 1;
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
            else if(winnerP2)
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

            Console.ForegroundColor = ConsoleColor.Gray;

            Print(7, 5, "Question: " + questionCounter);

            Console.ForegroundColor = ConsoleColor.Green;
            Print(9, 5, question1.text);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Print(11, 9, question1.a);
            Print(12, 9, question1.b);
            Print(13, 9, question1.c);
            Print(14, 9, question1.d);

            // 

            Print(15, 5, question1.correctAnswer);

            Console.ForegroundColor = ConsoleColor.White;
            Print(16, 5, "Player 1, choose an answer .. ");            

            if (IsAnsweredCorrect(question1.correctAnswer))
            {
                p1Answer = true;
            }
            else
            {

                p1Answer = false;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Print(18, 5, "Player 2, choose an answer .. ");

            if (IsAnsweredCorrect(question1.correctAnswer))
            {
                p2Answer = true;
            }
            else
            {

                p2Answer = false;
            }

            string correctAns = "The correct answer is: {0}";

            Console.ForegroundColor = ConsoleColor.Green;
            Print(20, 5, String.Format(correctAns, question1.correctAnswer));

            Console.ForegroundColor = ConsoleColor.Gray;
            Print(22, 5, "Next question ..");
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

        static void Tick(Object stateInfo)
        {
            Console.Write("\rTick Tack: {0}", DateTime.Now.ToString("h:mm:ss"));
        }
    }
}
