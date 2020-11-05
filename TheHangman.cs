using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Hangman_Game
{
    public class TheHangman
    {
        static int startLifeValue = 5;
        public static int getCurrentLife()
        {
            return currentLife_;
        }
        public static string findAWordToGuess()
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
        public static bool isUserGuessCorrect()
        {
            return currentWordToGuess_ == userGuess_;
        }
        public static void showDashes()
        {
            string dash = "_ ";
            int multiplier = currentWordToGuess_.Length;
            string dashes = string.Join(dash, new string[multiplier + 1]);
            Console.Write("Password: " + dashes + "\n");
        }
        public static string userGuess()
        {
            return userGuess_ = Console.ReadLine();
        }
        public static void showLife()
        {
            Console.Write("You have: " + currentLife_ + " chance(s). \n");
        }
        private static int currentLife_ = startLifeValue;
        private static string currentWordToGuess_ = findAWordToGuess();
        private static string userGuess_;
    }
}