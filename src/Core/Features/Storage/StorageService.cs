using System.Security.Cryptography;

namespace Core.Features.Storage;

public interface IStorageService
{
    int GetSampleCount();
    IEnumerable<string> GetAllFilesInStorage();
    string CalculateMD5(byte[] bytes);
    double GetSampleLength(byte[] bytes);
}

public class StorageService : IStorageService
{
    private readonly bool _isProduction;

    private string StoragePath => _isProduction 
        ? "/library" 
        : "../../test-library";

    public StorageService(bool isProduction)
    {
        _isProduction = isProduction;
    }

    public int GetSampleCount()
    {
        return GetAllFilesInStorage().Count();
    }

    public IEnumerable<string> GetAllFilesInStorage()
    {
        return Directory.GetFiles(
            StoragePath,
            "*.wav",
            SearchOption.AllDirectories);
    }

    public string CalculateMD5(byte[] bytes)
    {
        var hash = MD5.HashData(bytes);

        return Convert.ToBase64String(hash);
    }
    
    public double GetSampleLength(byte[] bytes)
    {
        var audioDataLength = BitConverter.ToInt32(bytes, 0x28);
        var sampleRate = BitConverter.ToInt32(bytes, 0x18);
        var numChannels = BitConverter.ToInt16(bytes, 0x16);
        var durationInSeconds = (double)audioDataLength / (sampleRate * numChannels * 2);

        return durationInSeconds;
    }
}