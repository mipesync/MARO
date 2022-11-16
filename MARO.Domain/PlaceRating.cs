namespace MARO.Domain
{
    public class PlaceRating
    {
        public string UserId { get; set; } = null!;
        public string PlaceId { get; set; } = null!;
        public int Rate { get; set; }

        public User User { get; set; } = null!;
    }
}
