using System.Drawing;
using System.Numerics;

namespace DripChip.Geo;

public class Polygon
{
    public Point[] Points;

    public bool HasIntersections()
    {
        var segments = Points.Select((point, i) => i switch
        {
            0 => new Segment
            {
                Start = point,
                End = Points.Last()
            },
            _ => new Segment
            {
                Start = point,
                End = Points[i - 1]
            }
        }).ToArray();

        foreach (var a in segments)
        {
            foreach (var b in segments)
            {
                if (a.Overlaps(b))
                    return true;
            }
        }

        return segments.Any(a => segments.Any(a.Overlaps));
    }


    public bool Overlaps(Polygon polygon)
    {
        foreach (var point in Points)
            if (point.Overlaps(polygon))
                return true;

        foreach (var point in polygon.Points)
            if (point.Overlaps(this))
                return true;

        return false;
    }
}