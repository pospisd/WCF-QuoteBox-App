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

        /// <summary>
        /// Update Author by passing an Author object.
        /// </summary>
        /// <param name="author"></param>
        public void UpdateAuthor(Author author)
        {
            Context.Entry(author).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Update Author by passing individual attributes.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="additionalTxt"></param>
        public void UpdateAuthor(
            int id,
            string firstName = null,
            string middleName = null,
            string lastName = null,
            string additionalTxt = null)
        {
            var author = Context.Authors.Find(id);

            if (firstName != null)
                author.FirstName = firstName;

            if (middleName != null)
                author.MiddleName = middleName;

            if (lastName != null)
                author.LastName = lastName;

            if (additionalTxt != null)
                author.AdditionalTxt = additionalTxt;

            // Make sure at least one parameter has been updated!
            if (firstName != null || middleName != null || lastName != null || additionalTxt != null)
                Context.SaveChanges();
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

