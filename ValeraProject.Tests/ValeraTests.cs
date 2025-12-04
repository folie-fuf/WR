using ValeraProject.Models;
using Xunit;

namespace ValeraProject.Tests
{
    public class ValeraTests
    {
        [Fact]
        public void GoToWork_Successful_WhenConditionsMet()
        {
            // Arrange
            var valera = new Valera { Mana = 30, Fatigue = 5 };
            
            // Act
            var result = valera.GoToWork();
            
            // Assert
            Assert.True(result);
            Assert.Equal(-5, valera.Cheerfulness);
            Assert.Equal(0, valera.Mana);
            Assert.Equal(100, valera.Money);
            Assert.Equal(75, valera.Fatigue);
        }

        [Fact]
        public void GoToWork_Fails_WhenHighAlcohol()
        {
            // Arrange
            var valera = new Valera { Mana = 60, Fatigue = 5 };
            
            // Act
            var result = valera.GoToWork();
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GoToWork_Fails_WhenHighFatigue()
        {
            // Arrange
            var valera = new Valera { Mana = 30, Fatigue = 15 };
            
            // Act
            var result = valera.GoToWork();
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ContemplateNature_IncreasesCheerfulness()
        {
            // Arrange
            var valera = new Valera { Mana = 50, Fatigue = 20 };
            
            // Act
            valera.ContemplateNature();
            
            // Assert
            Assert.Equal(1, valera.Cheerfulness);
            Assert.Equal(40, valera.Mana);
            Assert.Equal(30, valera.Fatigue);
        }

        [Fact]
        public void DrinkWineAndWatchTV_Successful_WhenHasMoney()
        {
            // Arrange
            var valera = new Valera { Money = 100, Health = 100 };
            
            // Act
            var result = valera.DrinkWineAndWatchTV();
            
            // Assert
            Assert.True(result);
            Assert.Equal(-1, valera.Cheerfulness);
            Assert.Equal(30, valera.Mana);
            Assert.Equal(10, valera.Fatigue);
            Assert.Equal(95, valera.Health);
            Assert.Equal(80, valera.Money);
        }

        [Fact]
        public void DrinkWineAndWatchTV_Fails_WhenNoMoney()
        {
            // Arrange
            var valera = new Valera { Money = 10 };
            
            // Act
            var result = valera.DrinkWineAndWatchTV();
            
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Sleep_RecoversHealth_WhenLowAlcohol()
        {
            // Arrange
            var valera = new Valera { Health = 20, Mana = 20, Fatigue = 80 };
            
            // Act
            valera.Sleep();
            
            // Assert
            Assert.Equal(90, valera.Health); // +70 к здоровью
            Assert.Equal(0, valera.Mana);
            Assert.Equal(10, valera.Fatigue);
        }

        [Fact]
        public void SingInMetro_BonusMoney_WhenAlcoholInRange()
        {
            // Arrange
            var valera = new Valera { Mana = 50, Money = 100 };
            
            // Act
            valera.SingInMetro();
            
            // Assert
            Assert.Equal(1, valera.Cheerfulness);
            Assert.Equal(60, valera.Mana);
            Assert.Equal(20, valera.Fatigue);
            Assert.Equal(150, valera.Money); // +50 бонус
        }

        [Fact]
        public void Stats_AreAlwaysInValidRange()
        {
            // Arrange
            var valera = new Valera();
            
            // Act - выполняем действия, которые могут вывести статистику за пределы
            valera.Health = 150;
            valera.Mana = -10;
            valera.Cheerfulness = 15;
            valera.Fatigue = 200;
            valera.Money = -50;
            
            // Вызываем любой метод для валидации
            valera.ContemplateNature();
            
            // Assert
            Assert.InRange(valera.Health, 0, 100);
            Assert.InRange(valera.Mana, 0, 100);
            Assert.InRange(valera.Cheerfulness, -10, 10);
            Assert.InRange(valera.Fatigue, 0, 100);
            Assert.True(valera.Money >= 0);
        }
    }
}