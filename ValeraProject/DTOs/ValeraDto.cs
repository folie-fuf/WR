namespace ValeraProject.DTOs
{
    public class ValeraDto
    {
        public int Id { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Cheerfulness { get; set; }
        public int Fatigue { get; set; }
        public int Money { get; set; }
        public int UserId { get; set; }
        public object? User { get; set; }
        public bool IsAlive => Health > 0;
    }

    public class CreateValeraDto
    {
        public int Health { get; set; } = 100;
        public int Mana { get; set; } = 0;
        public int Cheerfulness { get; set; } = 0;
        public int Fatigue { get; set; } = 0;
        public int Money { get; set; } = 100;
    }

    public class ActionRequestDto
    {
        public string Action { get; set; } = string.Empty;
    }
}