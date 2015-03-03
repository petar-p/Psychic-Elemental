namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Media;

    public class Questions
    {
        public struct Question
        {
            public string text;
            public string a;
            public string b;
            public string c;
            public string d;
            public char correctAnswer;
        }

        public const int GameWidth = FieldWidth + InfoPanelWidth + 3;
        public const int GameHeight = FieldHeigth + 2;
        public const int FieldWidth = 60;
        public const int FieldHeigth = 40;
        public const int InfoPanelWidth = 60;  // Info panel on the right

        public static string p1Input;
        public static string p2Input;
        public static bool p1Answer = false;
        public static bool p2Answer = false;
        public static bool isThereAWinner = false;

        public static int questionCounter = 0;
        public static int p1TiebreakScore = 0;
        public static int p2TiebreakScore = 0;
        
        public static Random randomGenerator = new Random();

        public static SoundPlayer markedAnswers = new SoundPlayer(@"..\..\sounds\marked-answers.wav");
        public static SoundPlayer correctAnswer = new SoundPlayer(@"..\..\sounds\correct-answer.wav");

        // Generate question
        public static void GenerateQuestion()
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

            Print(19, 9, question1.correctAnswer);

            Console.ForegroundColor = ConsoleColor.White;
            Print(22, 5,  p1Input + ", choose an answer .. ");
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
            Print(24, 5, p2Input + ", choose an answer .. ");

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

            markedAnswers.Play();
            Thread.Sleep(1500);

            string correctAns = "The correct answer is: {0}";

            Thread.Sleep(1500);

            Task.Run(() =>
            {
                // Use the code below parallel with the other code of the program
                // Play Sound alond with game play
                correctAnswer.Play();
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Print(26, 5, String.Format(correctAns, question1.correctAnswer));

            Console.ForegroundColor = ConsoleColor.Gray;
            Print(28, 5, "Next question ");
            Thread.Sleep(1500);
            Print(28, 18, " .");
            Thread.Sleep(1000);
            Print(28, 20, " .");
            Thread.Sleep(1000);
            Print(28, 22, " .");
            Thread.Sleep(1250);      
        }

        // Printing on custom position
        public static void Print(int row, int col, object data)
        {
            // Usually first receives Column, then Row, here we do the oposite
            Console.SetCursorPosition(col, row);
            Console.Write(data);
        }

        //printing questions
        public static void PrintQuestion(int row, int col, string text, int maxTextWidth)
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

        // Check if player answered correct
        public static bool IsAnsweredCorrect(char key)
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

        // Check if the answer is in valid format
        public static bool IsValidAnswer(char inputKey)
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
    }
}