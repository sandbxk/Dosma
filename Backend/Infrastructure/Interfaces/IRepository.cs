namespace Infrastructure.Interfaces;

public interface IRepository<T>
{
    List<T> All();
    T Create(T t);
    T Delete(int id);
    T Single(long id);
    T Update(long id, T model);
}
public interface IDatabase
{
    void BuildDB();
}

