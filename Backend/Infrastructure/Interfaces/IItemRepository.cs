using Backend.Infrastructure;
using Domain;

namespace Infrastructure.Interfaces;

/// <summary>
/// A repository interface for items.
/// <remarks>
/// This interface inherits from <see cref="IRepository{T}"/> and <see cref="IBulkRepository{T}"/>.
/// </remarks>
/// </summary>
public interface IItemRepository : IRepository<Item>, IBulkRepository<Item>
{
    /// <summary>
    /// Returns all items for a given grocery list.
    /// </summary>
    List<Item> GetAllForList(int id);
}
