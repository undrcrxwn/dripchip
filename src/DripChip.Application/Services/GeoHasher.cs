using System.Security.Cryptography;
using System.Text;
using DripChip.Application.Abstractions;

namespace DripChip.Application.Services;

public class GeoHasher : IGeoHasher
{
    private static readonly char[] Base32Chars = "0123456789bcdefghjkmnpqrstuvwxyz".ToCharArray();
    private static readonly int[] Bits = { 16, 8, 4, 2, 1 };

    public string EncodeV1(double latitude, double longitude, int precision = 12)
    {
        // Validate precision value.
        if (precision is < 1 or > 12)
            throw new ArgumentException("Precision must be between 1 and 12.");

        // Initialize latitude and longitude intervals.
        var latInterval = new[] { -90.0, 90.0 };
        var lonInterval = new[] { -180.0, 180.0 };

        // Initialize a StringBuilder to store the geohash.
        var geohash = new StringBuilder();
        var isEven = true;
        var bit = 0;
        var ch = 0;

        // Loop until the desired geohash length is reached.
        while (geohash.Length < precision)
        {
            double mid;

            // If it's an even iteration, adjust longitude interval and character value.
            if (isEven)
            {
                mid = (lonInterval[0] + lonInterval[1]) / 2;

                if (longitude >= mid)
                {
                    ch |= Bits[bit];
                    lonInterval[0] = mid;
                }
                else
                {
                    lonInterval[1] = mid;
                }
            }
            // If it's an odd iteration, adjust latitude interval and character value.
            else
            {
                mid = (latInterval[0] + latInterval[1]) / 2;

                if (latitude >= mid)
                {
                    ch |= Bits[bit];
                    latInterval[0] = mid;
                }
                else
                    latInterval[1] = mid;
            }

            // Toggle isEven flag.
            isEven = !isEven;

            // Increment bit index or reset and append character to geohash.
            if (bit < 4)
                bit++;
            else
            {
                geohash.Append(Base32Chars[ch]);
                bit = 0;
                ch = 0;
            }
        }

        return geohash.ToString();
    }

    public string EncodeV2(double latitude, double longitude, int precision = 12)
    {
        var hashV1 = EncodeV1(latitude, longitude);
        var encoded = Encoding.UTF8.GetBytes(hashV1);
        return Convert.ToBase64String(encoded);
    }

    public string EncodeV3(double latitude, double longitude, int precision = 12)
    {
        var hashV1 = EncodeV1(latitude, longitude);
        var encoded = Encoding.UTF8.GetBytes(hashV1);

        using var md5 = MD5.Create();
        var md5EncodedReversed = md5.ComputeHash(encoded).Reverse().ToArray();
        
        return Convert.ToBase64String(md5EncodedReversed);
    }
}