namespace Hb.Application.Responses
{
    public class ProductResponse
    {
        public string Id { get; set; }

        public string CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
