using Functions.Repository;

namespace Functions.Models
{
    public class OrderEntity : BaseEntity
    {
        public string Order { get; set; }
    }

    public class OrderEnrichedEntity : BaseEntity
    {
        public string OrderId { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }
    }
}
