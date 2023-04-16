namespace DripChip.Geo;

public class Point
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    // Given three collinear points p, q, r, the function checks if
    // point q lies on line segment 'pr'
    public bool Overlaps(Segment segment) =>
        Longitude <= Math.Max(segment.Start.Longitude, segment.End.Longitude) &&
        Longitude >= Math.Min(segment.Start.Longitude, segment.End.Longitude) &&
        Latitude <= Math.Max(segment.Start.Latitude, segment.End.Latitude) &&
        Latitude >= Math.Min(segment.Start.Latitude, segment.End.Latitude);


    public bool Overlaps(Polygon polygon)
    {
        if (polygon.Points.Any(x => x.Latitude == Latitude && x.Longitude == Longitude))
            return false;
        
        var points = polygon.Points;

        var result = false;

        var j = points.Count - 1;
        for (var i = 0; i < points.Count; i++)
        {
            if (points[i].Latitude < Latitude && points[j].Latitude >= Latitude || points[j].Latitude < Latitude && points[i].Latitude >= Latitude)
                if (points[i].Longitude + (Latitude - points[i].Latitude) / (points[j].Latitude - points[i].Latitude) *
                    (points[j].Longitude - points[i].Longitude) < Longitude)
                    result = !result;

            j = i;
        }

        return result;
    }
}