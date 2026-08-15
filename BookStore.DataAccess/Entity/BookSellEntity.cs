namespace BookStore.DataAccess.Entity
{
    public class BookSellEntity
    {
        public Guid Id { get; set; }
        public DateTime SellDate { get; set; }
        public List<BookSellItemEntity> Items { get; set; } = new();
    }
}
