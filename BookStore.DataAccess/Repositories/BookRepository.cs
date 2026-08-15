using BookStore.Core.Models;
using BookStore.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories
{
    public class BookRepository
    {
        private readonly BookStoreDBContext _context;

        public BookRepository(BookStoreDBContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            var bookEntities = await _context.Books
                .AsNoTracking()
                .ToListAsync();

            return bookEntities
                .Select(x => Book.Load(x.Id, x.Title, x.Author, x.Price))
                .ToList();
        }

        public async Task<Book?> GetById(Guid id)
        {
            var entity = await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return null;

            return Book.Load(entity.Id, entity.Title, entity.Author, entity.Price);
        }

        public async Task Create(Book book)
        {
            var bookEntity = new BookEntity
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price
            };

            _context.Books.Add(bookEntity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Guid id, string title, string author, decimal price)
        {
            await _context.Books
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.Title, x => title)
                    .SetProperty(x => x.Author, x => author)
                    .SetProperty(x => x.Price, x => price));
        }

        public async Task Delete(Guid id)
        {
            await _context.Books
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
