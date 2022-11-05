namespace MARO.Domain
{
    public class Group
    {
        public string Id { get; set; } = null!;
        public string? QRLink { get; set; }

        public IEnumerable<User> Users { get; set; } = null!;
    }
}
