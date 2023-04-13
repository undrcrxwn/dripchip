namespace DripChip.Geo;

public class Point
{
    public double X;
    public double Y;

    // Given three collinear points p, q, r, the function checks if
    // point q lies on line segment 'pr'
    public bool Overlaps(Segment segment) =>
        X <= Math.Max(segment.Start.X, segment.End.X) &&
        X >= Math.Min(segment.Start.X, segment.End.X) &&
        Y <= Math.Max(segment.Start.Y, segment.End.Y) &&
        Y >= Math.Min(segment.Start.Y, segment.End.Y);


    public bool Overlaps(Polygon polygon)
    {
        var points = polygon.Points;

        var result = false;

        var j = points.Length - 1;
        for (var i = 0; i < points.Length; i++)
        {
            if (points[i].Y < Y && points[j].Y >= Y || points[j].Y < Y && points[i].Y >= Y)
                if (points[i].X + (Y - points[i].Y) / (points[j].Y - points[i].Y) * (points[j].X - points[i].X) < X)
                    result = !result;

            j = i;
        }

        return result;
    }
}