namespace DripChip.Application.Abstractions;

public interface IGeoHasher
{
    public string EncodeV1(double latitude, double longitude, int precision = 12);
    public string EncodeV2(double latitude, double longitude, int precision = 12);
    public string EncodeV3(double latitude, double longitude, int precision = 12);
}