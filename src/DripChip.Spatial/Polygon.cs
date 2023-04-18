using System.Diagnostics;

namespace DripChip.Spatial;

[DebuggerDisplay("{Segments}")]
public class Polygon
{
    public IList<Point> Vertices { get; set; }
    public readonly IReadOnlyList<Segment> Segments;

    public Polygon(IList<Point> vertices)
    {
        Vertices = vertices;
        Segments = Vertices.Select((point, i) => i == 0
            ? new Segment { Start = Vertices.Last(), End = point }
            : new Segment { Start = Vertices[i - 1], End = point }).ToList();
    }

    public bool HasIntersections() => Segments.Any(a => Segments.Except(new[] { a }).Any(a.Intersects));

    public bool Overlays(Polygon polygon) =>
        Segments.Any(segment => segment.Intersects(polygon)) ||
        Vertices.Any(point => point.Overlays(polygon)) ||
        polygon.Vertices.Any(point => point.Overlays(this));
}