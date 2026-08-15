using CSharpFunctionalExtensions;

namespace BookStore.Core.Models
{
    public class BookSell
    {
        private readonly List<BookSellItem> _items = new();

        private BookSell(Guid id, DateTime sellDate)
        {
            Id = id;
            SellDate = sellDate;
        }

        public Guid Id { get; }
        public DateTime SellDate { get; }

        public IReadOnlyCollection<BookSellItem> Items => _items.AsReadOnly();

        public static Result<BookSell> Create()
        {
            return Result.Success(new BookSell(Guid.NewGuid(), DateTime.UtcNow));
        }

        public static BookSell Load(Guid id, DateTime sellDate, IEnumerable<BookSellItem> items)
        {
            var bookSell = new BookSell(id, sellDate);
            foreach (var item in items)
            {
                bookSell.LoadItem(item);
            }
            return bookSell;
        }

        private void LoadItem(BookSellItem item)
        {
            _items.Add(item);
        }


        public Result AddItem(Book book, int quantity)
        {
            var itemResult = BookSellItem.Create(book, quantity);
            if (itemResult.IsFailure)
                return itemResult;

            var existingItem = _items.FirstOrDefault(i => i.BookId == book.Id);
            if (existingItem != null)
            {
                int newQuantity = existingItem.Quantity + quantity;

                var updatedItem = BookSellItem.Create(book, newQuantity);
                if (updatedItem.IsFailure)
                    return updatedItem;

                _items.Remove(existingItem);
                _items.Add(updatedItem.Value);

                return Result.Success();
            }

            _items.Add(itemResult.Value);
            return Result.Success();
        }
    }
}
