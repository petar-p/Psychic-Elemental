namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    class Movement
    {

        public static char player1Character = '\u25A0';    //u263A
        public static char player2Character = '\u25A0';    //u263B

        public static bool winnerP1 = false;
        public static bool winnerP2 = false;

        public static int p1Move = 0;
        public static int p2Move = 0;

        // Coordinates of players moving positions
        #region
        // Coordinates of Player 1 moving positions on the map
        public static int[,] p1MovementCoords = new int[,] 
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
        public static int[,] p2MovementCoords = new int[,] 
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
        #endregion

        // Print players next positions
        public static void PrintPlayersNextPosition()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Questions.Print(p1MovementCoords[p1Move, 0], p1MovementCoords[p1Move, 1], player1Character);

            Console.ForegroundColor = ConsoleColor.Red;
            Questions.Print(p2MovementCoords[p2Move, 0], p2MovementCoords[p2Move, 1], player2Character);
        }

        public static void Player1Movement()
        {            
            if (Questions.p1Answer)
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
         
        public static void Player2Movement()
        {
            if (Questions.p2Answer)
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

        // Check for winner
        public static bool CheckForWinner()
        {
            if (p1Move == 13 && p2Move != 13)
            {
                // GAME OVER! The winner is Player 1
                winnerP1 = true;    // flag that Player 1 is winner
                Questions.isThereAWinner = true;
            }
            else if (p2Move == 13 && p1Move != 13)
            {
                // GAME OVER! The winner is Player 2
                winnerP2 = true;    // flag that Player 2 is winner
                Questions.isThereAWinner = true;
            }
            else if (p1Move == 13 && p2Move == 13)
            {
                // Both players are at the end of the labyrinth, that means equal score                

                // TODO: Decide how to name the winner... penalties or something else
                Questions.isThereAWinner = false;
            }
            return Questions.isThereAWinner;
        }
    }
}