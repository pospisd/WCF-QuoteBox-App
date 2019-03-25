using System;
using System.ComponentModel.DataAnnotations;
using QuoteBoxData.EF;

namespace QuoteBoxData.EF
{
    public partial class Author
    {
        //public virtual ICollection<Author> Authors { get; set; } = new HashSet<Author>();
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public void RemoveRecord(int authorId)
        {
            // Find an Author to delete by primary key.
            using (var context = new QuoteBoxEntities())
            {
                // See if we have it.
                Author authorToDelete = context.Authors.Find(authorId);
                if (authorToDelete != null)
                {
                    Console.WriteLine("TODO: Implement Author delete with orphaned quotes...");
                    context.Authors.Remove(authorToDelete);
                    context.SaveChanges();
                }

            }
        }
    }
}