namespace Common.Interfaces
{
    /// <summary>
    /// Interface of datastore read service.
    /// </summary>
    /// <typeparam name="TEntity">Data transfer object.</typeparam>
    public interface IReadRepository<TEntity>
    {
        /// <summary>
        /// Gets entity by id.
        /// </summary>
        /// <param name="id">Entity unique indentifier.</param>
        /// <returns>Entity dto.</returns>
        Task<TEntity> GetByIdAsync(Guid id);
    }
}
