using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Hangman_Game
{
    public class TheHangman
    {
        static int startLifeValue = 5;
        public static void showDashes()
        {
            string dash = "_ ";
            int multiplier = currentWordToGuess_.Length;
            string dashes = string.Join(dash, new string[multiplier + 1]);
            Console.Write("Password: " + dashes + "\n");
        }
        public static void showLife()
        {
            Console.Write("You have: " + currentLife_ + " chance(s). \n");
        }
        public static void playARound()
        {
            while (currentLife_ > 0)
            {
                askUserToGuessALetterOrAWord();
                showLife();
            }
        }
        private static string findAWordToGuess()
        {
            var words = new Dictionary<string, string>(){
            {"Albania", "Tirana"},
            {"Belarus", "Minsk"},
            {"Croatia", "Zagreb"},
            {"Denmark", "Copenhagen"},
            {"Estonia", "Tallinn"}
            };
            var rnd = new Random();
            var randomWord = words.ElementAt(rnd.Next(0, words.Count));
            string randomKey = randomWord.Key;
            string randomValue = randomWord.Value;
            return randomWord.Value;
        }
        private static void askUserToGuessALetterOrAWord()
        {
            Console.Write("Do You want to guess a letter or a word? \n Type L - for letter, W - for word and press enter. \n");
            var decision = Console.ReadLine();
            makeDecision(decision);
        }
        private static void makeDecision(string decision)
        {
            if (decision.ToUpper() == "L" || decision.ToUpper() == "W")
            {
                var guess = Console.ReadLine();
                if (!passwordValidator(guess))
                {
                    makeNotInAWordList(decision, guess);
                    --currentLife_;
                    if (currentLife_ == 0)
                    {
                        Console.Write("Game over!\n");
                    }
                    else
                    {
                        Console.Write("You have lost a chance. Try again. \n");
                    }
                }
                else
                {
                    Console.Write("Nice shot. \n");
                }
            }
            else
            {
                Console.Write("Try again. \n");
            }
        }
        private static void makeNotInAWordList(string decision, string guess)
        {
            if (decision.ToUpper() == "L" && !notInWord_.Contains(guess))
            {
                notInWord_.Add(guess.ToUpper());
                notInWord_.Sort();
                showNotInAWordList();
            }
        }
        private static void showNotInAWordList()
        {
            Console.Write("Not in a word: ");
            notInWord_.ForEach(Console.Write);
            Console.Write("\n");
        }
        private static bool passwordValidator(string guess)
        {
            return guess == currentWordToGuess_ || currentWordToGuess_.Contains(guess);
        }
        private static int currentLife_ = startLifeValue;
        private static string currentWordToGuess_ = findAWordToGuess();
        private static string temporaryWordToGuess_ = currentWordToGuess_;
        private static List<string> notInWord_ = new List<string>();
    }
}