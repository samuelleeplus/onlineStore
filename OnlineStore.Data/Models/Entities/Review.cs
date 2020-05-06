namespace OnlineStore.Data.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ReviewProduct { get; set; }
    }
}