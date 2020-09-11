using CRM.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace CRM.Services.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Entities;
        protected readonly DbSet<T> Dbset;

        public GenericRepository(DbContext context)
        {
            Entities = context;
            Dbset = context.Set<T>();
        }

        /// <summary>
        /// Возвращает коллекцию, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <returns>IQueryable<T></returns>
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = Dbset.AsNoTracking().Where(predicate);
            return query;
        }

        /// <summary>
        /// Возвращает коллекцию c вложенными коллекциями, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <param name="include">массив перечислений вложенностей</param>
        /// <returns>int</returns>
        public IQueryable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params string[] include)
        {
            IQueryable<T> query = this.Dbset.AsNoTracking();
            Func<IQueryable<T>, string, IQueryable<T>> func = (current, inc) => current.Include(inc);
            query = include.Aggregate(query, func).AsNoTracking();

            //foreach (var inc in include)
            //{
            //    query.Include(inc);
            //}

            return query.Where(predicate);
        }

        /// <summary>
        /// Возвращает отсортированную коллекцию c вложенными коллекциями, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <param name="page">страница</param>
        /// <param name="pageSize">количество элементов</param>
        /// <param name="include">массив перечислений вложенностей</param>
        /// <param name="sortColumn">колонка сортировки</param>
        /// <param name="sortDirection">направление сортировки</param>
        /// <returns>IQueryable<T></returns>
        public IQueryable<T> GetSortedWithInclude(Expression<Func<T, bool>> predicate, int page, int pageSize, string sortColumn, string sortDirection, params string[] include)
        {
            IQueryable<T> query = this.Dbset.AsNoTracking();
            Func<IQueryable<T>, string, IQueryable<T>> func = (current, inc) => current.Include(inc);
            query = include.Aggregate(query, func).AsNoTracking();

            return query.Where(predicate)
                .OrderBy($@"{sortColumn} {sortDirection}")
                .Skip(page * pageSize)
                .Take(pageSize);
        }

        /// <summary>
        /// Возвращает количество элементов, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <param name="include">массив перечислений вложенностей</param>
        /// <returns>IQueryable<T></returns>
        public int GetCount(Expression<Func<T, bool>> predicate, params string[] include)
        {
            IQueryable<T> query = this.Dbset.AsNoTracking();
            Func<IQueryable<T>, string, IQueryable<T>> func = (current, inc) => current.Include(inc);
            query = include.Aggregate(query, func).AsNoTracking();

            return query.Count(predicate);
        }

    }
}
