using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace The_Hangman_Game
{
    public class TheHangman
    {
        static int startLifeValue = 5;
        static int numberOflinesInFile = 183;
        static int numberOfBestPlayers = 10;
        static string guessFilePath = "countries_and_capitals.txt.txt";
        static string tempHighScorePath = "temp_high_score.txt";
        static string highScorePath = "high_score.txt";
        static string recordSeparator = " | ";
        public static void playAGame()
        {
            showHelloScreen();
            Console.ReadKey();
            do
            {
                Console.Clear();
                playARound();
                Console.Clear();
                showTheBestPlayers();
                resetGame();
                Console.Write("Do You want to play again Y/N?\n");
            }
            while (Console.ReadLine().ToUpper() == "Y");
        }

        private static string dashes()
        {
            string dash = "_ ";
            int multiplier = currentWordToGuess_.Length;
            return string.Join(dash, new string[multiplier + 1]);
        }
        private static void showHelloScreen()
        {
            Console.Write("Welcome in the Hangman Game. Press enter to continue...");
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
        private static void showTheBestPlayers()
        {
            if(File.Exists(highScorePath))
            {
                StreamReader reader = new StreamReader(highScorePath);
                Console.WriteLine("Factor" + recordSeparator + "Name" + recordSeparator
                                + "Date" + recordSeparator
                                + "Round time [s]" + recordSeparator
                                + "Trials" + recordSeparator + "Word");
                while (reader.Peek() >= 0)
                {
                    Console.WriteLine(reader.ReadLine());
                }
                reader.Close();
            }
        }
        private static void showWordAfterGuessing()
        {   
            if (tempCleaningWordToGuess_ == "")
            {
                tempDisplayingGuessingWord_ = currentWordToGuess_;
            }
            else
            {
                tempDisplayingGuessingWord_ = currentWordToGuess_;
                if (tempDisplayingGuessingWord_.Contains(tempGuess_))
                {
                    tempDisplayingGuessingWord_.Replace(tempGuess_, "_");
                }
            }     
            Console.Write(tempDisplayingGuessingWord_ + "\n");
        }        
        private static string findAWordToGuess()
        {      
            StreamReader reader = new StreamReader(guessFilePath);
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
                showWordAfterGuessing();
                showLife();
                if (currentLife_ == 1)
                {
                    showAHint();
                }
                if (winConditions())
                {
                    Console.Clear();
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
                    if (tempCleaningWordToGuess_.Contains(tempGuess_))
                    {
                        tempCleaningWordToGuess_ = tempCleaningWordToGuess_.Replace(tempGuess_, "");
                    }  
                    Console.Clear();          
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
            Console.Clear();
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
                tempCleaningWordToGuess_ = "";
            }
        }
        private static bool winConditions()
        {
            return tempCleaningWordToGuess_ == "";
        }
        private static void addAHighScore()
        {
            Console.WriteLine("Do You want to be in Hall of Fame? Y/N\n");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                Console.WriteLine("Please, give me your name.\n");
                var name = Console.ReadLine();
                StreamWriter writer = File.AppendText(tempHighScorePath);
                writer.WriteLine(countWinFactor().ToString() + recordSeparator + name + recordSeparator
                                 + DateTime.Now.ToString() + recordSeparator
                                 + roundTimeCounter_.ElapsedMilliseconds/1000 + "s" + recordSeparator
                                 + guessingCounter_ + recordSeparator + currentWordToGuess_);
                writer.Close();
                highScoreValidator();
            }
        }
        private static void highScoreValidator()
        {
            if(File.Exists(tempHighScorePath))
            {
                var contents = File.ReadAllLines(tempHighScorePath);
                Array.Sort(contents);
                Array.Resize(ref contents, numberOfBestPlayers);
                File.WriteAllLines(highScorePath, contents);
            }
        }
        private static double countWinFactor()
        {
            double counter = guessingCounter_;
            double timer = roundTimeCounter_.ElapsedMilliseconds/1000;
            return Math.Round(counter / timer, 3);
        }
        private static void resetGame()
        {
            currentLife_ = startLifeValue;
            guessingCounter_ = 0;
            roundTimeCounter_ = new Stopwatch();
            currentWordToGuess_ = findAWordToGuess();
            tempCleaningWordToGuess_ = currentWordToGuess_;
            tempDisplayingGuessingWord_ = dashes();
            notInWord_.Clear();
        }
        private static int currentLife_ = startLifeValue;
        private static int guessingCounter_ = 0;
        private static string currentState_;
        private static string currentWordToGuess_ = findAWordToGuess();
        private static string tempCleaningWordToGuess_ = currentWordToGuess_;
        private static string tempDisplayingGuessingWord_ = dashes();
        private static string tempGuess_ = " ";
        private static List<string> notInWord_ = new List<string>();
        private static Stopwatch roundTimeCounter_ = new Stopwatch();
    }
}