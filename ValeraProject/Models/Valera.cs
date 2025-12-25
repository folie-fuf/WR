using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeraProject.Models
{
    public class Valera
    {
        [Key]
        public int Id { get; set; }
        
        [Range(0, 100)]
        public int Health { get; set; } = 100;
        
        [Range(0, 100)]
        public int Mana { get; set; } = 0;
        
        [Range(-10, 10)]
        public int Cheerfulness { get; set; } = 0;
        
        [Range(0, 100)]
        public int Fatigue { get; set; } = 0;
        
        public int Money { get; set; } = 100;
        
        // Внешний ключ для пользователя
        public int UserId { get; set; }
        
        // Навигационное свойство
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Добавляем свойство IsAlive, которое не сохраняется в БД, а вычисляется
        [NotMapped]
        public bool IsAlive => Health > 0;


        public bool GoToWork()
        {
            if (Mana >= 50 || Fatigue >= 10)
                return false;

            Cheerfulness -= 5;
            Mana = Math.Max(0, Mana - 30);
            Money += 100;
            Fatigue += 70;

            ValidateStats();
            return true;
        }

        public void ContemplateNature()
        {
            Cheerfulness += 1;
            Mana = Math.Max(0, Mana - 10);
            Fatigue += 10;

            ValidateStats();
        }

        public bool DrinkWineAndWatchTV()
        {
            if (Money < 20)
                return false;

            Cheerfulness -= 1;
            Mana += 30;
            Fatigue += 10;
            Health = Math.Max(0, Health - 5);
            Money -= 20;

            ValidateStats();
            return true;
        }

        public bool GoToBar()
        {
            if (Money < 100)
                return false;

            Cheerfulness += 1;
            Mana += 60;
            Fatigue += 40;
            Health = Math.Max(0, Health - 10);
            Money -= 100;

            ValidateStats();
            return true;
        }

        public bool DrinkWithMarginals()
        {
            if (Money < 150)
                return false;

            Cheerfulness += 5;
            Health = Math.Max(0, Health - 80);
            Mana += 90;
            Fatigue += 80;
            Money -= 150;

            ValidateStats();
            return true;
        }

        public void SingInMetro()
        {
            Cheerfulness += 1;
            Mana += 10;
            Fatigue += 20;

            if (Mana > 40 && Mana < 70)
            {
                Money += 50;
            }
            else
            {
                Money += 10;
            }

            ValidateStats();
        }

        public void Sleep()
        {
            if (Mana < 30)
            {
                Health = Math.Min(100, Health + 90);
            }

            if (Mana > 70)
            {
                Cheerfulness -= 3;
            }

            Mana = Math.Max(0, Mana - 50);
            Fatigue = Math.Max(0, Fatigue - 70);

            ValidateStats();
        }

        private void ValidateStats()
        {
            Health = Math.Max(0, Math.Min(100, Health));
            Mana = Math.Max(0, Math.Min(100, Mana));
            Cheerfulness = Math.Max(-10, Math.Min(10, Cheerfulness));
            Fatigue = Math.Max(0, Math.Min(100, Fatigue));
            Money = Math.Max(0, Money);
        }
    }
}