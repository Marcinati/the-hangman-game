using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace The_Hangman_Game
{
    public class TheHangman
    {
        static int startLifeValue = 5;
        static int numberOflinesInFile = 183;
        static string guessFilePath = "countries_and_capitals.txt.txt";
        static string highScorePath = "high_score.txt";
        static string recordSeparator = " | ";
        public static void playAGame()
        {
            do
            {
                playARound();
                resetGame();
                Console.Write("Do you want to play again Y/N?\n");
            }
            while (Console.ReadLine().ToUpper() == "Y");
        }

        private static string dashes()
        {
            string dash = "_ ";
            int multiplier = currentWordToGuess_.Length;
            return string.Join(dash, new string[multiplier + 1]);
        }
        private static void showAHint()
        {
            Console.Write("HINT: The capitol of " + currentState_ + "\n");
        }
        private static void showEndOfGameScreen()
        {
            Console.Write("You guessed the capital after " + guessingCounter_ + " letter(s). ");
            Console.Write("It took you " + roundTimeCounter_.ElapsedMilliseconds/1000 + " seconds. \n");
        }
        private static void showLife()
        {
            Console.Write("You have: " + currentLife_ + " chance(s).\n");
        }
        private static void showNotInAWordList()
        {
            Console.Write("Not in a word: ");
            notInWord_.ForEach(Console.Write);
            Console.Write("\n");
        }
        private static void showWordAfterGuessing()
        {        
            Console.Write(temporaryWordAfterGuessing_ + "\n");
        }        
        private static string findAWordToGuess()
        {      
            string path = guessFilePath;
            StreamReader reader = new StreamReader(path);
            var rnd = new Random();
            int numberOfRandomLine = rnd.Next(0, numberOflinesInFile - 1);
            int tempLineNumber = 0;
            while (tempLineNumber != numberOfRandomLine) {
                ++tempLineNumber;
                var tempLine = reader.ReadLine();
                if (tempLineNumber == numberOfRandomLine)
                {
                    string [] split = tempLine.Split(new Char [] {'|'});
                    currentState_ = split[0].Trim(' ').ToUpper();
                    currentWordToGuess_ = split[1].Trim(' ').ToUpper();
                }
            }
            reader.Close();
            return currentWordToGuess_;
        }
        private static void playARound()
        {
            roundTimeCounter_.Start();
            showWordAfterGuessing();
            while (currentLife_ > 0)
            {
                askUserToGuessALetterOrAWord();
                showLife();
                if (currentLife_ == 1)
                {
                    showAHint();
                }
                if (winConditions())
                {
                    Console.Write("Congratulations! You won.\n");
                    showEndOfGameScreen();
                    addAHighScore();
                    break;
                }
            }
            roundTimeCounter_.Stop();
        }
        private static void askUserToGuessALetterOrAWord()
        {
            Console.Write("Do You want to guess a letter or a word? \n>>>Type L - for letter, W - for word and press enter. \n");
            var decision = Console.ReadLine();
            pickDecision(decision);
        }
        private static void pickDecision(string decision)
        {
            if (decision.ToUpper() == "L")
            {
                guessLetter();
            }
            if (decision.ToUpper() == "W")
            {
                guessWord();
            }
            else
            {
                Console.Write("Try again. \n");
            }
        }
        private static void guessLetter()
        {
            var guess = Console.ReadLine().ToUpper();
                ++guessingCounter_;
                if (!passwordValidator(guess))
                {
                    makeNotInAWordList(guess);
                    showNotInAWordList();
                    decrementLifeAndCheckCondition();
                }
                else
                {
                    temporaryWordToGuess_ = temporaryWordToGuess_.Replace(guess, ""); //guess can't be equal ""
                    Console.Write("Nice shot. \n");
                }
        }
        private static void guessWord()
        {
            var guess = Console.ReadLine().ToUpper();
            ++guessingCounter_;
            checkUltimateWin(guess);
            if (!passwordValidator(guess))
            {
                decrementLifeAndCheckCondition();
                decrementLifeAndCheckCondition();
            }
        }
        private static void decrementLifeAndCheckCondition()
        {
            if (currentLife_ == 0)
            {
                Console.Write("Game over!\n");
            }
            else
            {
                --currentLife_;
                Console.Write("You have lost a chance.\n");
            }
        }
        private static void makeNotInAWordList(string guess)
        {
            if (!notInWord_.Contains(guess))
            {
                notInWord_.Add(guess.ToUpper());
                notInWord_.Sort();
            }
        }
        private static bool passwordValidator(string guess)
        {
            return currentWordToGuess_.Contains(guess);
        }
        private static void checkUltimateWin(string guess)
        {
            if (guess == currentWordToGuess_)
            {
                temporaryWordToGuess_ = "";
            }
        }
        private static bool winConditions()
        {
            return temporaryWordToGuess_ == "";
        }
        private static void addAHighScore()
        {
            Console.WriteLine("Do you want to be in Hall of Fame? Y/N\n");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                Console.WriteLine("Please, give me your name.\n");
                var name = Console.ReadLine();
                StreamWriter writer = File.AppendText(highScorePath);
                writer.WriteLine(name + recordSeparator + DateTime.Now.ToString() + recordSeparator + roundTimeCounter_.ElapsedMilliseconds/1000 + "s"
                                 + recordSeparator + guessingCounter_ + recordSeparator + currentWordToGuess_);
                writer.Close();
            }
        }
        private static void resetGame()
        {
            currentLife_ = startLifeValue;
            guessingCounter_ = 0;
            roundTimeCounter_ = new Stopwatch();
            currentWordToGuess_ = findAWordToGuess();
            temporaryWordToGuess_ = currentWordToGuess_;
            temporaryWordAfterGuessing_ = dashes();
            notInWord_.Clear();
        }
        private static int currentLife_ = startLifeValue;
        private static int guessingCounter_ = 0;
        private static string currentState_;
        private static string currentWordToGuess_ = findAWordToGuess();
        private static string temporaryWordToGuess_ = currentWordToGuess_;
        private static string temporaryWordAfterGuessing_ = dashes();
        private static List<string> notInWord_ = new List<string>();
        private static Stopwatch roundTimeCounter_ = new Stopwatch();
    }
}