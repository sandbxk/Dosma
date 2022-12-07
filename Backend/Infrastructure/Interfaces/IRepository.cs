namespace Infrastructure.Interfaces;

/// <summary>
/// A base CRUD interface.
/// </summary>
public interface IRepository<T>
{
    /// <summary>
    /// Returns all items.
    /// </summary>
    List<T> All();

    /// <summary>
    /// Creates a new item.
    /// </summary>
    T Create(T t);

    /// <summary>
    /// Returns a single item.
    /// </summary>
    T Single(long id);

    /// <summary>
    /// Updates an item.
    /// </summary>
    T Update(T t);

    /// <summary>
    /// Deletes an item.
    /// </summary>
    bool Delete(int id);
}

public interface IDatabase
{
    void BuildDB();
}

