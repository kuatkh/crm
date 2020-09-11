using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CRM.Services.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Возвращает коллекцию, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Возвращает коллекцию c вложенными коллекциями, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <param name="include">массив перечислений вложенностей</param>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params string[] include);

        /// <summary>
        /// Возвращает отсортированную коллекцию c вложенными коллекциями, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <param name="include">массив перечислений вложенностей</param>
        /// <param name="page">страница</param>
        /// <param name="pageSize">количество элементов</param>
        /// <param name="sortColumn">колонка сортировки</param>
        /// <param name="sortDirection">направление сортировки</param>
        /// <returns>IQueryable<T></returns>
        IQueryable<T> GetSortedWithInclude(Expression<Func<T, bool>> predicate, int page, int pageSize, string sortColumn, string sortDirection, params string[] include);

        /// <summary>
        /// Возвращает коллекцию c вложенными коллекциями, где
        /// </summary>
        /// <param name="predicate">дерево выражений</param>
        /// <param name="include">массив перечислений вложенностей</param>
        /// <returns>int</returns>
        int GetCount(Expression<Func<T, bool>> predicate, params string[] include);

    }
}
