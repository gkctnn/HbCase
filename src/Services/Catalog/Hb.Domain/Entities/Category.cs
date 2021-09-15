using Hb.Domain.Entities.Base;
using System.Collections.Generic;

namespace Hb.Domain.Entities
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
