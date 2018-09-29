using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Interface defining base data access functions.
    /// </summary>
    /// <typeparam name="T">Type of the object.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Selects all objects from the repository.
        /// </summary>
        /// <returns>List of objects.</returns>
        IQueryable<T> Select();

        /// <summary>
        /// Selects objects from the repository.
        /// </summary>
        /// <param name="predicate">Condition for selection.</param>
        /// <returns>List of objects.</returns>
        IList<T> Select(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Fetches the specified object.
        /// </summary>
        /// <param name="id">The ID of the object to fetch.</param>
        /// <returns>The object requested or null if it cannot be found.</returns>
        T Get(int id);

        /// <summary>
        /// Fetches the specified object.
        /// </summary>
        /// <param name="predicate">The predicate used to locate the object.</param>
        /// <returns>The object requested or null if it cannot be found</returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Saves an object. Will add or update object.
        /// </summary>
        /// <param name="objectToSave">The object to be saved</param>
        /// <returns>ID of the saved object.</returns>
        int Save(T objectToSave);

        /// <summary>
        /// Deletes an object.
        /// </summary>
        /// <param name="objectToDelete">The object to be deleted.</param>
        void Delete(T objectToDelete);
    }
}