using System;

namespace The_Hangman_Game
{
    class TheHangmanGame
    {
        static void Main(string[] args)
        {
            Console.Title = "The_Hangman_Game";
            Console.ForegroundColor = ConsoleColor.Green;

            TheHangman.showDashes();
            TheHangman.playARound();

            Console.ReadLine();
        }
        
    }
}
