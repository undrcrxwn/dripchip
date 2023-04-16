namespace DripChip.Geo;

public class Segment
{
    public Point Start;
    public Point End;

    public enum Orientation
    {
        Collinear,
        Clockwise,
        Counterclockwise
    }

    // To find orientation of ordered triplet (p, q, r).
    private Orientation GetOrientation(Point p, Point q, Point r)
    {
        // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
        var val =
            (q.Latitude - p.Latitude) * (r.Longitude - q.Longitude) -
            (q.Longitude - p.Longitude) * (r.Latitude - q.Latitude);

        return val switch
        {
            0 => Orientation.Collinear,
            > 0 => Orientation.Clockwise,
            _ => Orientation.Counterclockwise
        };
    }

    // The main function that returns true if line segment 's1.Starts1.End'
// and 's2.Starts2.End' intersect.
    public bool Overlaps(Segment segment)
    {
        // Find the four orientations needed for general and
        // special cases
        var o1 = GetOrientation(Start, End, segment.Start);
        var o2 = GetOrientation(Start, End, segment.End);
        var o3 = GetOrientation(segment.Start, segment.End, Start);
        var o4 = GetOrientation(segment.Start, segment.End, End);

        // Ignore ends
        if (Start == segment.Start ^ Start == segment.End ||
            End == segment.Start ^ End == segment.End)
            return false;

        return (o1 != o2 && o3 != o4) ||
               (o1 == 0 && segment.Start.Overlaps(this)) ||
               (o2 == 0 && segment.End.Overlaps(this)) ||
               (o3 == 0 && Start.Overlaps(segment)) ||
               (o4 == 0 && End.Overlaps(segment));
    }
}