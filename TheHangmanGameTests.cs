using Xunit;

namespace The_Hangman_Game
{
    public class TheHangmanGameTests
    {        
        [Fact]
        public void CurrentLifeCounterShouldShowMaxLifeAtTheBegininngOfTheStage()
        {
            int MaxLife = 5;
            Assert.Equal(TheHangman.getCurrentLife(), MaxLife);
        }
    }
}