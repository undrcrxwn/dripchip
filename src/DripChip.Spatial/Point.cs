using System.Diagnostics;

namespace DripChip.Spatial;

[DebuggerDisplay("{Longitude}, {Latitude}")]
public class Point : IEquatable<Point>
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public Point(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    // Given three collinear points p, q, r, the function checks if
    // point q lies on line segment 'pr'
    public bool Overlays(Segment segment) =>
        Longitude <= Math.Max(segment.Start.Longitude, segment.End.Longitude) &&
        Longitude >= Math.Min(segment.Start.Longitude, segment.End.Longitude) &&
        Latitude <= Math.Max(segment.Start.Latitude, segment.End.Latitude) &&
        Latitude >= Math.Min(segment.Start.Latitude, segment.End.Latitude) &&
        (Longitude - segment.Start.Longitude) * (segment.End.Latitude - segment.Start.Latitude) -
        (segment.End.Longitude - segment.Start.Longitude) * (Latitude - segment.Start.Latitude) == 0;

    public bool Overlays(Polygon polygon)
    {
        if (polygon.Segments.Any(Overlays))
            return false;

        var result = false;

        for (int i = 0, j = polygon.Vertices.Count - 1; i < polygon.Vertices.Count; j = i++)
        {
            var a = polygon.Vertices[i].Latitude >= Latitude;
            var b = polygon.Vertices[j].Latitude >= Latitude;
            var c = polygon.Vertices[j].Longitude - polygon.Vertices[i].Longitude;
            var d = Latitude - polygon.Vertices[i].Latitude;
            var e = polygon.Vertices[j].Latitude - polygon.Vertices[i].Latitude;
            var f = polygon.Vertices[i].Longitude;

            if (a != b && Longitude <= c * d / e + f)
                result = !result;
        }

        return result;
    }

    public bool Equals(Point? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Point)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Longitude, Latitude);

    public static bool operator ==(Point a, Point b) => a.Equals(b);
    public static bool operator !=(Point a, Point b) => !(a == b);
}