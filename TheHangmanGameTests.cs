using Xunit;

namespace The_Hangman_Game
{
    public class TheHangmanGameTests
    {        
        [Fact]
        public void CurrentLifeCounterShouldShowMaxLifeAtTheBegininngOfTheStage()
        {
            int maxLife = 5;
            Assert.Equal(TheHangman.getCurrentLife(), maxLife);
        }
    }
}