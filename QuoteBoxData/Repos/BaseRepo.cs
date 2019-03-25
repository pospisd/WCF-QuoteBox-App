using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using QuoteBoxDAL.EF;

namespace QuoteBoxDAL.Repos
{
    public abstract class BaseRepo<T> where T : class, new()
    {
        public QuoteBoxEntities Context { get; } = new QuoteBoxEntities();
        protected DbSet<T> Table;


        /*********************************************************************
         * 
         *  SaveChanges() Helper Methods:
         * 
         *********************************************************************/
        internal int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Throw when there is a concurrency error. For now just rethrow the exception.
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Throw when database update fails.
                // Examine the inner exception(s) for additional details and affected objects
                // for now, just retrhow the exception.
                throw;
            }
            catch (CommitFailedException ex)
            {
                // Handle transaction failures from here for now, just rethrow the exception.
                throw;
            }
            catch (Exception ex)
            {
                // Some other exception happened, and should be handled...
                throw;
            }
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Throw when there is a concurrency error. For now just rethrow the exception.
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Throw when database update fails.
                // Examine the inner exception(s) for additional details and affected objects
                // for now, just retrhow the exception.
                throw;
            }
            catch (CommitFailedException ex)
            {
                // Handle transaction failures from here for now, just rethrow the exception.
                throw;
            }
            catch (Exception ex)
            {
                // Some other exception happened, and should be handled...
                throw;
            }
        }

        /*********************************************************************
         * 
         *  Retrieving Records:
         * 
         *********************************************************************/

        public T GetOne(int? id) => Table.Find(id);
        public Task<T> GetOneAsync(int? id) => Table.FindAsync(id);
        public List<T> GetAll() => Table.ToList();
        public Task<List<T>> GetAllAsync() => Table.ToListAsync();


        /*********************************************************************
         * 
         *  Retrieving Records With SQL:
         * 
         *********************************************************************/

        public List<T> ExecuteQuery(string sql) => Table.SqlQuery(sql).ToList();
        public Task<List<T>> ExecuteQueryAsync(string sql) => Table.SqlQuery(sql).ToListAsync();
        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
            => Table.SqlQuery(sql, sqlParametersObjects).ToList();
        public Task<List<T>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects)
            => Table.SqlQuery(sql).ToListAsync();

        /*********************************************************************
         * 
         * Adding Records:
         * 
         *********************************************************************/

        public int Add(T entity)
        {
            Table.Add(entity);
            return SaveChanges();
        }

        public Task<int> AddAsync(T entity)
        {
            Table.Add(entity);
            return SaveChangesAsync();
        }

        public int AddRange(IList<T> entities)
        {
            Table.AddRange(entities);
            return SaveChanges();
        }

        public Task<int> AddRangeAsync(IList<T> entities)
        {
            Table.AddRange(entities);
            return SaveChangesAsync();
        }

        /*********************************************************************
         * 
         * Updating Records:
         * 
         *********************************************************************/
        public int Save(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public Task<int> SaveAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        /*********************************************************************
         * 
         * Deleting Records:
         * 
         *********************************************************************/
        public int Delete(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }

    public abstract class BaseRepo : IDisposable
    {
        protected QuoteBoxEntities Context { get; } = new QuoteBoxEntities();


        /*********************************************************************
         * 
         *  SaveChanges() Helper Methods:
         * 
         *********************************************************************/
        internal int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Throw when there is a concurrency error. For now just rethrow the exception.
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Throw when database update fails.
                // Examine the inner exception(s) for additional details and affected objects
                // for now, just retrhow the exception.
                throw;
            }
            catch (CommitFailedException ex)
            {
                // Handle transaction failures from here for now, just rethrow the exception.
                throw;
            }
            catch (Exception ex)
            {
                // Some other exception happened, and should be handled...
                throw;
            }
        }

        internal async Task<int> SaveChangesAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Throw when there is a concurrency error. For now just rethrow the exception.
                throw;
            }
            catch (DbUpdateException ex)
            {
                // Throw when database update fails.
                // Examine the inner exception(s) for additional details and affected objects
                // for now, just retrhow the exception.
                throw;
            }
            catch (CommitFailedException ex)
            {
                // Handle transaction failures from here for now, just rethrow the exception.
                throw;
            }
            catch (Exception ex)
            {
                // Some other exception happened, and should be handled...
                throw;
            }
        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                Context.Dispose();
                // Free any managed objects here.
                //
            }

            // Free any managed objects here.
            //
            disposed = true;
        }
    }

    public class QuoteRepo : BaseRepo<Quote>, IRepo<Quote>, IDisposable
    {
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                Context.Dispose();
                // Free any managed objects here.
                //
            }

            // Free any managed objects here.
            //
            disposed = true;
        }

        public QuoteRepo()
        {
            Table = Context.Quotes;
        }

        public int Delete(int id, byte[] timestamp)
        {
            Context.Entry(new Quote()
            {
                QuoteId = id
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timestamp)
        {
            Context.Entry(new Quote()
            {
                QuoteId = id
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }

    public class AuthorRepo : BaseRepo<Author>, IRepo<Author>, IDisposable
    {
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                Context.Dispose();
                // Free any managed objects here.
                //
            }

            // Free any managed objects here.
            //
            disposed = true;
        }

        public AuthorRepo()
        {
            Table = Context.Authors;
        }

        public int Delete(int id, byte[] timestamp)
        {
            Context.Entry(new Author()
            {
                AuthorId = id
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int id, byte[] timestamp)
        {
            Context.Entry(new Author()
            {
                AuthorId = id
            }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}
