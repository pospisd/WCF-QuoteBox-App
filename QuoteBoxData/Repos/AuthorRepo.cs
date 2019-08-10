using System;
using System.Collections.Generic;
using System.Linq;
using QuoteBoxData.EF;

namespace QuoteBoxData.Repos
{
    public class AuthorRepo
    {
        private QuoteBoxEntities Context;

        public AuthorRepo(QuoteBoxEntities context)
        {
            this.Context = context;
        }

        public IEnumerable<Author> GetAuthors()
        {
            return Context.Authors.ToList();
        }

        public Author GetAuthorByID(int id)
        {
            return Context.Authors.Find(id);
        }

        public void InsertAuthor(Author author)
        {
            Context.Authors.Add(author);
        }

        public void DeleteAuthor(int authorID)
        {
            Author author = Context.Authors.Find(authorID);
            Context.Authors.Remove(author);
        }

        public void UpdateAuthor(Author author)
        {
            Context.Entry(author).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        private bool dispose = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.dispose)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.dispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
