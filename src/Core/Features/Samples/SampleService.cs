using System.Security;
using Core.Data;

namespace Core.Features.Samples;

public interface ISampleService
{
    Task<int> GetSampleCount();
    Task MarkAllExistingAsProcessing();
    Task<Sample> GetSampleFromMD5(string md5);
    Task UpdateSample(Sample sample);
    Task AddSample(Sample sample);
    Task DeleteAllProcessing();
}

public class SampleService : ISampleService
{
    private readonly IDbFactory _dbFactory;

    public SampleService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<int> GetSampleCount()
    {
        var db = await _dbFactory.GetDatabase();

        return await db.GetCollection<Sample>().CountAsync();
    }

    public async Task MarkAllExistingAsProcessing()
    {
        var db = await _dbFactory.GetDatabase();
        var sampleCollection = db.GetCollection<Sample>();

        using var transaction = await db.BeginTransactionAsync();
        
        var samples = (await sampleCollection.FindAllAsync()).ToList();

        foreach (var sample in samples)
        {
            sample.Processing = true;
            await sampleCollection.UpdateAsync(sample);
        }

        await transaction.CommitAsync();
    }

    public async Task<Sample> GetSampleFromMD5(string md5)
    {
        var db = await _dbFactory.GetDatabase();
        var sample = await db.GetCollection<Sample>()
            .FindOneAsync(x => x.Hash == md5);

        return sample;
    }

    public async Task UpdateSample(Sample sample)
    {
        var db = await _dbFactory.GetDatabase();
        
        await db.GetCollection<Sample>().UpdateAsync(sample);
    }

    public async Task AddSample(Sample sample)
    {
        var db = await _dbFactory.GetDatabase();

        await db.GetCollection<Sample>().InsertAsync(sample);
    }

    public async Task DeleteAllProcessing()
    {
        var db = await _dbFactory.GetDatabase();

        await db.GetCollection<Sample>()
            .DeleteManyAsync(x => x.Processing);
    }
}