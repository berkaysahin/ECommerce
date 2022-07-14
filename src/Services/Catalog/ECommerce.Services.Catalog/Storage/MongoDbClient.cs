﻿using System;
using System.Linq.Expressions;
using ECommerce.Services.Catalog.Interfaces;
using ECommerce.Services.Catalog.Models;
using ECommerce.Services.Catalog.Settings;
using MongoDB.Driver;

namespace ECommerce.Services.Catalog.Storage;

public class MongoDbClient<T> : IMongoDbClient<T> where T : class
{
    private IMongoDatabase _database;
    private IMongoCollection<T> _collection;

    public MongoDbClient()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("CatalogDb");
    }

    public void Setup(string collectionName)
    {
        _collection = _database.GetCollection<T>(collectionName);
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await _collection.Find(expression).ToListAsync();
    }
    
    public async Task InsertOneAsync(T model)
    {
        await _collection.InsertOneAsync(model);
    }
}