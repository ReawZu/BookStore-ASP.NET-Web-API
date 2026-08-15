using CSharpFunctionalExtensions;

namespace BookStore.Core.Models
{
    public class BookSellItem
    {
        private BookSellItem(Guid id, Guid bookId, int quantity, decimal priceAtCurrentTime)
        {
            Id = id;
            BookId = bookId;
            Quantity = quantity;
            PriceAtCurrentTime = priceAtCurrentTime;
        }

        public Guid Id { get; }
        public Guid BookId { get; }
        public int Quantity { get; }
        public decimal PriceAtCurrentTime { get; }

        public static Result<BookSellItem> Create(Book book, int quantity)
        {
            if (book == null)
                return Result.Failure<BookSellItem>("Книга не может быть пустой");

            if (quantity <= 0)
                return Result.Failure<BookSellItem>("Количество книг должно быть больше нуля");

            return Result.Success(new BookSellItem(Guid.NewGuid(), book.Id, quantity, book.Price));
        }

        public static BookSellItem Load(Guid id, Guid bookId, int quantity, decimal priceAtCurrentTime)
        {
            return new BookSellItem(id, bookId, quantity, priceAtCurrentTime);
        }

    }
}
