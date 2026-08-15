using BookStore.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess
{
    public class BookStoreDBContext : DbContext
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> options)
            : base(options)
        {
            
        }

        public DbSet<BookEntity> Books { get; set; }
        public DbSet<BookSellEntity> BookSells { get; set; }
        public DbSet<BookSellItemEntity> BookSellItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreDBContext).Assembly);
        }
    }
}
