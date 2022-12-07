using Infrastructure.Interfaces;

namespace Backend.Infrastructure
{
    /// <summary>
    /// A repository that supports bulk operations.
    /// </summary>
    public interface IBulkRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Creates a new entry for each item in the list.
        /// </summary>
        /// <returns> The number of entries created. </returns>
        int BulkCreate(List<T> t);

        /// <summary>
        /// Updates entries for each item in the list.
        /// </summary>
        /// <returns> The number of entries updated. </returns>
        int BulkUpdate(List<T> t);

        /// <summary>
        /// Deletes entries for each item in the list.
        /// </summary>
        /// <returns> The number of entries deleted. </returns>
        int BulkDelete(List<T> t);
    }
}