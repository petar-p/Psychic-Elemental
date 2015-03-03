namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    class GenericGameClass
    {
        public static void Main()
        {
            // Setting Game Title
            Console.Title = "C# Scramble";

            // Set Encoding
            Console.OutputEncoding = Encoding.UTF8;

            // Removing unusable space
            Console.WindowWidth = Questions.GameWidth;
            Console.BufferWidth = Questions.GameWidth;
            Console.WindowHeight = Questions.GameHeight + 2;
            Console.BufferHeight = Questions.GameHeight + 2;

            Draw.DrawMenuScreen();
            Draw.DrawLogo();
            Console.Clear();

            // Draw menu
            Draw.DrawMenuScreen();
            Draw.PrintMenu();

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Questions.Print(38, 90, Movement.player1Character);
            Console.ForegroundColor = ConsoleColor.Red;
            Questions.Print(38, 94, Movement.player2Character);

            while (true)
            {
                Console.CursorVisible = false;

                // Draw game field
                Draw.DrawBorders();

                Draw.PrintPlayerInfo();

                // Draw Labyrinth
                Draw.DrawLabyrinth();

                Movement.PrintPlayersNextPosition();

                Draw.PrintAnswersToWin();

                // Check for winner
                if ((Movement.p1Move == 13 || Movement.p2Move == 13) &&  Questions.questionCounterp2==Questions.questionCounterp1 )
                {
                    if (Movement.CheckForWinner())
                    {
                        break;  // GAME OVER ! There's a winner
                    }
                    else
                    {
                        // Both players are at the end of the labyrinth, that means equal score
                        Draw.GameOver();
                        // TODO: Decide how to name the winner... penalties or something else
                        continue;
                    }
                }

                if (Questions.turnOfPLayer % 2 == 0)
                {
                    Questions.GenerateQuestion(Questions.p1Input, Questions.turnOfPLayer);
                    Movement.PlayerMovement(Questions.turnOfPLayer);
                }
                if (Questions.turnOfPLayer % 2 == 1)
                {
                    Questions.GenerateQuestion(Questions.p2Input, Questions.turnOfPLayer);
                    Movement.PlayerMovement(Questions.turnOfPLayer);
                }

                Questions.turnOfPLayer++;

                Console.Clear();
            }

            Draw.GameOver();
            Console.ReadLine();
            Console.Clear();

            Draw.PrintCredits();
            Console.ReadLine();
        }
    }
}
