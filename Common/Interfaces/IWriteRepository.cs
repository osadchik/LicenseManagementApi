namespace Common.Interfaces
{
    /// <summary>
    /// Interface of datastore write service.
    /// </summary>
    /// <typeparam name="TEntity">Data transfer object.</typeparam>
    public interface IWriteRepository<TEntity>
    {
        /// <summary>
        /// Creates or updates entity in the datastore.
        /// </summary>
        /// <param name="entity">Entity to be created.</param>
        /// <returns>Saved user dto.</returns>
        Task<TEntity> SaveAsync(TEntity entity);

        /// <summary>
        /// Deletes entity from the datastore.
        /// </summary>
        /// <param name="id">Entity unique identifier.</param>
        /// <returns>Deleted entity dto.</returns>
        Task<TEntity> DeleteAsync(string id);
    }
}
