using BookStore.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.DataAccess.Configuration
{
    public class BookSellItemConfiguration : IEntityTypeConfiguration<BookSellItemEntity>
    {
        public void Configure(EntityTypeBuilder<BookSellItemEntity> builder)
        {
            builder.ToTable("BookSellItem");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PriceAtCurrentTime)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();
        }
    }
}
