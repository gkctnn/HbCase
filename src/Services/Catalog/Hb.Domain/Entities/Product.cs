using Hb.Domain.Entities.Base;

namespace Hb.Domain.Entities
{
    public class Product : Entity
    {
        public string CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
