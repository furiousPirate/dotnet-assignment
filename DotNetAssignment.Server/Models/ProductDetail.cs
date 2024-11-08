namespace DotNetAssignment.Server.Models
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public required string ImageData { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Quantity { get; set; }
        public required double Price { get; set; }
        public required DateTime Date { get; set; }
    }
}
