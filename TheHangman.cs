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
        public static void playAGame()
        {
            do
            {
                playARound();
                resetGame();
                Console.Write("Do you want to play again Y/N?\n");
            } while (Console.ReadLine().ToUpper() == "Y");
        }

        private static string dashes()
        {
            string dash = "_ ";
            int multiplier = currentWordToGuess_.Length;
            return string.Join(dash, new string[multiplier + 1]);
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
            Console.Write("The capitol of the " + currentCapital_ + " is " + temporaryWordAfterGuessing_ + "\n");
        }        
        private static string findAWordToGuess()
        {      
            var words = new Dictionary<string, string>(){
            {"Albania", "TIRANA"},
            {"Belarus", "MINSK"},
            {"Croatia", "ZAGREB"},
            {"Denmark", "COPENHAGEN"},
            {"Estonia", "TALLINN"}
            };
            var rnd = new Random();
            var randomWord = words.ElementAt(rnd.Next(0, words.Count));
            string randomKey = randomWord.Key;
            currentCapital_ = randomKey;
            string randomValue = randomWord.Value;
            return randomWord.Value;
        }
        private static void playARound()
        {
            roundTimeCounter_.Start();
            showWordAfterGuessing();
            while (currentLife_ > 0)
            {
                askUserToGuessALetterOrAWord();
                showLife();
                if (winConditions())
                {
                    Console.Write("Congratulations! You won.\n");
                    showEndOfGameScreen();
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
        private static string currentCapital_;
        private static string currentWordToGuess_ = findAWordToGuess();
        private static string temporaryWordToGuess_ = currentWordToGuess_;
        private static string temporaryWordAfterGuessing_ = dashes();
        private static List<string> notInWord_ = new List<string>();
        private static Stopwatch roundTimeCounter_ = new Stopwatch();
    }
}