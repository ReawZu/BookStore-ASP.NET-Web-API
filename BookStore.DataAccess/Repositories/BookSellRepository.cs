using BookStore.Core.Models;
using BookStore.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories
{
    public class BookSellRepository
    {
        private readonly BookStoreDBContext _context;

        public BookSellRepository(BookStoreDBContext context)
        {
            _context = context;
        }

        public async Task<List<BookSell>> GetAll()
        {
            var sellEntities = await _context.BookSells
                .Include(x => x.Items)
                .AsNoTracking()
                .ToListAsync();

            return sellEntities.Select(entity =>
            {
                var domainItems = entity.Items.Select(i => BookSellItem.Load(i.Id, i.BookId, i.Quantity, i.PriceAtCurrentTime));

                return BookSell.Load(entity.Id, entity.SellDate, domainItems);
            }).ToList();
        }

        public async Task<BookSell?> GetById(Guid id)
        {
            var entity = await _context.BookSells
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) 
                return null;

            var domainItems = entity.Items.Select(i =>
                BookSellItem.Load(i.Id, i.BookId, i.Quantity, i.PriceAtCurrentTime));

            return BookSell.Load(entity.Id, entity.SellDate, domainItems);
        }

        public async Task Create(BookSell bookSell)
        {
            var sellEntity = new BookSellEntity
            {
                Id = bookSell.Id,
                SellDate = bookSell.SellDate,

                Items = bookSell.Items.Select(item => new BookSellItemEntity
                {
                    Id = item.Id,
                    BookSellId = bookSell.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    PriceAtCurrentTime = item.PriceAtCurrentTime
                }).ToList()
            };

            _context.BookSells.Add(sellEntity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(BookSell bookSell)
        {
            var existingEntity = await _context.BookSells
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == bookSell.Id);

            if (existingEntity == null) 
                return;

            existingEntity.SellDate = bookSell.SellDate;

            _context.BookSellItems.RemoveRange(existingEntity.Items);

            existingEntity.Items = bookSell.Items.Select(item => new BookSellItemEntity
            {
                Id = item.Id,
                BookSellId = bookSell.Id,
                BookId = item.BookId,
                Quantity = item.Quantity,
                PriceAtCurrentTime = item.PriceAtCurrentTime
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            await _context.BookSells
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
