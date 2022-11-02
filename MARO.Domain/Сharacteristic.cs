namespace MARO.Domain
{
    public class Сharacteristic
    {
        public int Id { get; set; }
        public List<int>? Ages { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string? Place { get; set; }
        public string UserId { get; set; } = null!;
        public int CriterionId { get; set; } = 3;

        public User User { get; set; } = null!;
        public Criterion Criterion { get; set; } = null!;
    }
}
