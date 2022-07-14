using System.Linq.Expressions;

namespace ECommerce.Services.Catalog.Interfaces;

public interface IMongoDbClient<T> where T : class
{
    void Setup(string collectionName);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task<T> FindByIdAsync(Expression<Func<T, bool>> expression);
    Task InsertOneAsync(T model);
}
