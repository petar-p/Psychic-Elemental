namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Media;
    using System.Threading.Tasks;
    using System.Threading;

    class GenericGameClass
    {
        public static SoundPlayer introSound = new SoundPlayer(@"..\..\sounds\intro.wav");
        public static SoundPlayer ambienceSound = new SoundPlayer(@"..\..\sounds\ambience.wav");
        public static SoundPlayer endCredits = new SoundPlayer(@"..\..\sounds\end.wav");     

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

            Task.Run(() =>
            {
                // Use the code below parallel with the other code of the program
                // Play Sound alond with game play
                introSound.Play();
            });

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

                Task.Run(() =>
                {
                    // Use the code below parallel with the other code of the program
                    // Play Sound alond with game play
                    ambienceSound.PlayLooping();
                });

                // Draw Labyrinth
                Draw.DrawLabyrinth();

                Movement.PrintPlayersNextPosition();

                Draw.PrintAnswersToWin();

                // Check for winner
                if ((Movement.p1Move == 13 || Movement.p2Move == 13))
                {
                    if (Movement.CheckForWinner())
                    {
                        break;  // GAME OVER ! There's a winner
                    }
                    else
                    {
                        // Both players are at the end of the labyrinth, that means equal score
                        if ((Movement.p1Move > 13 || Movement.p2Move > 13))
                        {
                            Movement.p1Move = 13;
                            Movement.p2Move = 13;
                        }
                        
                        Draw.Tiebreak();
                        Questions.GenerateQuestion();
                        Draw.PrintTieBreakScore();

                        if (Questions.p1Answer == true && Questions.p2Answer == true)
                        {
                            Questions.p1TiebreakScore++;
                            Questions.p2TiebreakScore++;
                        }

                        Draw.PrintTieBreakScore();

                        // Both players answered correct and the tiebreak continues
                        if (Questions.p1Answer == true && Questions.p2Answer == true)
                        {
                            continue;
                        }
                        else if (Questions.p1Answer == true && Questions.p2Answer == false)
                        {
                            // Player 1 wins the tie break and the game
                            Task.Run(() =>
                            {
                                // Use the code below parallel with the other code of the program
                                // Play Sound alond with game play
                                Draw.winnerAnnounce.Play();
                            });
                            Questions.Print(39, 23, "GAME OVER ! THE WINNER IS: " + Questions.p1Input);                            

                            Thread.Sleep(28000);
                            Console.Clear();

                            Task.Run(() =>
                            {
                                // Use the code below parallel with the other code of the program
                                // Play Sound alond with game play
                                endCredits.Play();
                            });

                            Draw.PrintCredits();
                            Console.ReadLine();

                        }
                        else if (Questions.p1Answer == false && Questions.p2Answer == true)
                        {
                            // Player 2 wins the tiebreak and the game
                            Task.Run(() =>
                            {
                                // Use the code below parallel with the other code of the program
                                // Play Sound alond with game play
                                Draw.winnerAnnounce.Play();
                            });

                            Questions.Print(39, 23, "GAME OVER ! THE WINNER IS: " + Questions.p2Input);                            

                            Thread.Sleep(28000);
                            Console.Clear();

                            Task.Run(() =>
                            {
                                // Use the code below parallel with the other code of the program
                                // Play Sound alond with game play
                                endCredits.Play();
                            });

                            Draw.PrintCredits();
                            Console.ReadLine();
                        }
                        else
                        {
                            // Both players gave wrong answers answers and the tiebreak continues
                            continue;
                        }
                    }
                }
                
                Questions.GenerateQuestion();

                Movement.Player1Movement();
                Movement.Player2Movement();                

                Console.Clear();
            }

            Draw.GameOver();
            Thread.Sleep(28000);
            Console.Clear();

            Task.Run(() =>
            {
                // Use the code below parallel with the other code of the program
                // Play Sound alond with game play
                endCredits.Play();
            });

            Draw.PrintCredits();
            Console.ReadLine();
        }
    }
}
