namespace Core.Features.Samples;

public class Sample
{
    public int Id { get; set; }
    public string Hash { get; init; } = string.Empty;
    public int FileSize { get; set; }
    public double SampleLength { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public bool Processing { get; set; }
}