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
            String randomKey = randomWord.Key;
            String randomValue = randomWord.Value;
            return randomWord.Value;
        }
        public static bool isUserGuessCorrect(string userGuess)
        {
            return currentWordToGuess_ == userGuess;
        }
        private static int currentLife_ = startLifeValue;
        private static string currentWordToGuess_ = findAWordToGuess();
    }
}