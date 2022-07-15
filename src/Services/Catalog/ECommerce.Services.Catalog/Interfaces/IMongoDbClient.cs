using System.Linq.Expressions;
using MongoDB.Driver;

namespace ECommerce.Services.Catalog.Interfaces;

public interface IMongoDbClient<T> where T : class
{
    void Setup(string collectionName);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task<T> FindByIdAsync(Expression<Func<T, bool>> expression);
    Task InsertOneAsync(T model);
    Task<T> FindOneAndReplace(Expression<Func<T, bool>> expression, T model);
    Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> expression);
}
