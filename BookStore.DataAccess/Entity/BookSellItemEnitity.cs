namespace BookStore.DataAccess.Entity
{
    public class BookSellItemEntity
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid BookSellId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtCurrentTime { get; set; }
    }
}
