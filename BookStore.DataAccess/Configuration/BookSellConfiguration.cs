using BookStore.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.DataAccess.Configuration
{
    public class BookSellConfiguration : IEntityTypeConfiguration<BookSellEntity>
    {
        public void Configure(EntityTypeBuilder<BookSellEntity> builder)
        {
            builder.ToTable("BookSell");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Items)
                   .WithOne()
                   .HasForeignKey("BookSellId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
