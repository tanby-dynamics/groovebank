using Core.Features.Samples;
using LiteDB.Async;

namespace Core.Data;

public interface IDbFactory
{
    Task<LiteDatabaseAsync> GetDatabase();
}

public class DbFactory : IDbFactory
{
    private readonly bool _isProduction;

    public DbFactory(bool isProduction)
    {
        _isProduction = isProduction;
    }
    private string DbPath => _isProduction
        ? "/data/groovebank.db"
        : "./groovebank-dev.db";

    public async Task<LiteDatabaseAsync> GetDatabase()
    {
        var db = new LiteDatabaseAsync(DbPath);
        
        // set up some indices
        //var sampleCollection = db.GetCollection<Sample>();
        //await sampleCollection.EnsureIndexAsync(x => x.Hash);
        
        return db;
    }
}


