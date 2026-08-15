using CSharpFunctionalExtensions;

namespace BookStore.Core.Models
{
    public class Book
    {
        public const int MAX_TITLE_LENGTH = 100;

        private Book(Guid id, string title, string author, decimal price)
        {
            Id = id; 
            Title = title; 
            Author = author;
            Price = price;
        }

        private Book() { }

        public Guid Id { get; }
        public string Title { get; } = string.Empty;
        public string Author { get; } = string.Empty;
        public decimal Price { get; }

        public static Result<Book> Create(string title, string author, decimal price)
        {
            var validationResult = Validate(title, author, price);
            if (validationResult.IsFailure)
                return Result.Failure<Book>(validationResult.Error);

            return Result.Success(new Book(Guid.NewGuid(), title, author, price));
        }

        public static Book Load(Guid id, string title, string author, decimal price)
        {
            return new Book(id, title, author, price);
        }

        private static Result Validate(string title, string author, decimal price)
        {
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
                return Result.Failure($"{nameof(title)} не может быть пустым или превышать {MAX_TITLE_LENGTH} символов");

            if (string.IsNullOrEmpty(author) || author.Length > MAX_TITLE_LENGTH)
                return Result.Failure($"{nameof(author)} не может быть пустым или превышать {MAX_TITLE_LENGTH} символов");

            if (price < 0)
                return Result.Failure($"{nameof(price)} не может быть отрицательным");

            return Result.Success();
        }

    }
}
