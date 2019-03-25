using System.Data.Entity;

namespace QuoteBoxData.EF
{
    public partial class QuoteBoxEntities : DbContext
    {
        public QuoteBoxEntities()
            : base("name=QuoteBoxEntities")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(e => e.Quotes)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);
        }
    }
}
