namespace Infrastructure.Interfaces
{
    using Backend.Infrastructure;
    using Domain;

    /// <summary>
    /// A repository interface for grocery lists.
    /// <remarks>
    /// This interface inherits from <see cref="IRepository{T}"/>.
    /// </remarks>
    /// </summary>
    public interface IGroceryListRepository : IRepository<GroceryList>
    {
        /// <summary>
        /// Returns all grocery lists for a given user.
        /// </summary>
        List<GroceryList> GetAllForUser(int id);
    }
}