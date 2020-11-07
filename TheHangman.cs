using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace The_Hangman_Game
{
    public class TheHangman
    {
        const string guessFilePath = "countries_and_capitals.txt.txt";
        const string highScorePath = "high_score.txt";
        const string tempHighScorePath = "temp_high_score.txt";
        const string recordSeparator = " | ";
        const int numberOfBestPlayers = 10;
        const int numberOfLinesInGuessFile = 183;
        const int roundingWinFactor = 3;
        const int startLifeValue = 5;

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
        private static void showHelloScreen()
        {
            Console.Write("Welcome in the Hangman Game. Press any button to continue...");
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
            for (int i = 0; i < currentWordToGuess_.Length; ++i)
            {
                if (currentWordToGuess_[i] == ' ')
                {
                    tempDisplayingGuessingWord_[i] = ' ';
                }
                if (currentWordToGuess_[i].ToString() == tempGuess_)
                {
                    var putInDisplay = tempGuess_.ToCharArray();
                    tempDisplayingGuessingWord_[i] = putInDisplay[0];
                }
            }
            for (int j = 0; j < currentWordToGuess_.Length; ++j)
            {
                Console.Write(tempDisplayingGuessingWord_[j]);
                Console.Write(' ');
            }
            Console.Write("\n");
        }        
        private static string findAWordToGuess()
        {      
            StreamReader reader = new StreamReader(guessFilePath);
            var rnd = new Random();
            int numberOfRandomLine = rnd.Next(0, numberOfLinesInGuessFile - 1);
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
            while (currentLife_ > 0)
            {
                showWordAfterGuessing();
                askUserToGuessALetterOrAWord();
                showNotInAWordList();
                showLife();
                if (currentLife_ == 1)
                {
                    showAHint();
                }
                if (winConditions())
                {
                    Console.Clear();
                    Console.Write("Congratulations! You won. That was a " + currentWordToGuess_ + ". \n");
                    showEndOfGameScreen();
                    addAHighScore();
                    break;
                }
            }
            roundTimeCounter_.Stop();
        }
        private static void askUserToGuessALetterOrAWord()
        {
            Console.Write("Do You want to guess a letter or a word? \n>>>Type L - for letter, W - for word and press enter.\n");
            char decision = Console.ReadLine().ToUpper()[0];
            pickDecision(decision);
        }
        private static void pickDecision(char decision)
        {
            if (decision == 'L')
            {
                guessLetter();
            }
            if (decision == 'W')
            {
                guessWord();
            }
            else
            {
                Console.Write("Try again.\n");
            }
        }
        private static void guessLetter()
        {
            char guess = Console.ReadLine().ToUpper()[0];
            tempGuess_ = guess.ToString();
            ++guessingCounter_;
            if (!passwordValidator())
            {
                makeNotInAWordList();
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
            tempGuess_ = Console.ReadLine().ToUpper();
            ++guessingCounter_;
            checkUltimateWin();
            if (!passwordValidator())
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
        private static void makeNotInAWordList()
        {
            if (!notInWord_.Contains(tempGuess_))
            {
                notInWord_.Add(tempGuess_);
                notInWord_.Sort();
            }
        }
        private static bool passwordValidator()
        {
            return currentWordToGuess_.Contains(tempGuess_);
        }
        private static void checkUltimateWin()
        {
            if (tempGuess_ == currentWordToGuess_)
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
            return Math.Round(counter / timer, roundingWinFactor);
        }
        private static void resetGame()
        {
            currentLife_ = startLifeValue;
            guessingCounter_ = 0;
            roundTimeCounter_ = new Stopwatch();
            currentWordToGuess_ = findAWordToGuess();
            tempCleaningWordToGuess_ = currentWordToGuess_;
            notInWord_.Clear();
            tempDisplayingGuessingWord_ = prepareDisplayWord(); 
            tempGuess_ = "0";           
        }
        private static char[] prepareDisplayWord()
        {
            char[] displayDashes = new char[currentWordToGuess_.Length];
            for (int i = 0; i < currentWordToGuess_.Length; ++i)
            {
                displayDashes[i] = '_';
            }
            return displayDashes;
        }
        private static int currentLife_ = startLifeValue;
        private static int guessingCounter_ = 0;
        private static string currentState_ = "";
        private static string currentWordToGuess_ = findAWordToGuess();
        private static string tempCleaningWordToGuess_ = currentWordToGuess_;
        private static char[] tempDisplayingGuessingWord_ = prepareDisplayWord();
        private static char[] userShots_ = new char[50];
        private static string tempGuess_ = "0";
        private static List<string> notInWord_ = new List<string>();
        private static Stopwatch roundTimeCounter_ = new Stopwatch();
    }
}