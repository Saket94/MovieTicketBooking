using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.DataAccessLayer
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private CinemaTicketBookingEntities _context = null;
        private DbSet<T> table = null;
        public GenericRepository()
        {
            this._context = new CinemaTicketBookingEntities();
            table = _context.Set<T>();
        }
        public GenericRepository(CinemaTicketBookingEntities _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public IEnumerable<T> GetMatched(Expression<Func<T, bool>> predicate)
        {
            return table.Where(predicate);
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

       
    }
}