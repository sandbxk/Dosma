namespace Infrastructure.Interfaces;

using Domain;

public interface IRepository<T>
{
    List<T> All();
    T Create(T t);
    bool Delete(int id);
    T Single(int id);
    T Update(T model);
}


public interface IDatabase
{
    void BuildDB();
}

