using System.Diagnostics;

namespace DripChip.Spatial;

[DebuggerDisplay("{Start} <-> {End}")]
public class Segment : IEquatable<Segment>
{
    public Point Start { get; set; }
    public Point End { get; set; }

    public enum Orientation
    {
        Collinear,
        Clockwise,
        Counterclockwise
    }

    // Finds orientation of ordered triplet (p, q, r).
    private Orientation GetOrientation(Point p, Point q, Point r)
    {
        // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
        var value =
            (q.Latitude - p.Latitude) * (r.Longitude - q.Longitude) -
            (q.Longitude - p.Longitude) * (r.Latitude - q.Latitude);

        return value switch
        {
            0 => Orientation.Collinear,
            > 0 => Orientation.Clockwise,
            _ => Orientation.Counterclockwise
        };
    }

    public bool Intersects(Segment segment)
    {
        var o1 = GetOrientation(Start, End, segment.Start);
        var o2 = GetOrientation(Start, End, segment.End);
        var o3 = GetOrientation(segment.Start, segment.End, Start);
        var o4 = GetOrientation(segment.Start, segment.End, End);

        if ((Start == segment.Start) ^ (Start == segment.End) ||
            (End == segment.Start) ^ (End == segment.End))
            return false;

        return (o1 != o2 && o3 != o4) ||
               (o1 == 0 && segment.Start.Overlays(this)) ||
               (o2 == 0 && segment.End.Overlays(this)) ||
               (o3 == 0 && Start.Overlays(segment)) ||
               (o4 == 0 && End.Overlays(segment));
    }

    public bool Intersects(Polygon polygon) => polygon.Segments.Any(Intersects);

    public bool Equals(Segment? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Segment)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Start, End);

    public static bool operator ==(Segment a, Segment b) => a.Equals(b);
    public static bool operator !=(Segment a, Segment b) => !(a == b);
}