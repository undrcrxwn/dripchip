namespace DripChip.Geo.UnitTests;

public class PathIntersections
{
    [Fact]
    public void TriangleHasNoIntersections()
    {
        var path = new Polygon
        {
            Points = new Point[]
            {
                new() { X = 0, Y = 0 },
                new() { X = 0.5, Y = 1 },
                new() { X = 1, Y = 0 }
            }
        };

        Assert.False(path.HasIntersections());
    }
    
    [Fact]
    public void ConcavePolygonHasNoIntersections()
    {
        var path = new Polygon
        {
            Points = new Point[]
            {
                new() { X = 0, Y = 1 },
                new() { X = -1, Y = -3 },
                new() { X = -0.5, Y = 1 },
                new() { X = -5, Y = 3 },
                new() { X = 3, Y = 5 },
                new() { X = -1.5, Y = 2.5 },
                new() { X = 5, Y = 3.5 },
                new() { X = 2, Y = -7 }
            }
        };

        Assert.False(path.HasIntersections());
    }
    
    [Fact]
    public void SinglePointOnSegmentIsNotIntersection()
    {
        var path = new Polygon
        {
            Points = new Point[]
            {
                new() { X = -1, Y = -1 },
                new() { X = 1, Y = -1 },
                new() { X = 1, Y = 1 },
                new() { X = 0, Y = -1 },
                new() { X = -1, Y = 1 }
            }
        };

        Assert.False(path.HasIntersections());
    }
    
    [Fact]
    public void CollinearPathHasIntersections()
    {
        var path = new Polygon
        {
            Points = new Point[]
            {
                new() { X = 0, Y = 0 },
                new() { X = 0.5, Y = 0.5 },
                new() { X = 2, Y = 2 },
                new() { X = 5, Y = 5 }
            }
        };

        Assert.True(path.HasIntersections());
    }
    
    [Fact]
    public void InfinitySymbolHasIntersections()
    {
        var path = new Polygon
        {
            Points = new Point[]
            {
                new() { X = 0, Y = 0 },
                new() { X = 0, Y = 1 },
                new() { X = 1, Y = 0 },
                new() { X = 1, Y = 1 }
            }
        };

        Assert.True(path.HasIntersections());
    }
    
    [Fact]
    public void TwoSquaresOverlap()
    {
        var a = new Polygon
        {
            Points = new Point[]
            {
                new() { X = -1, Y = -1 },
                new() { X = -1, Y = 1 },
                new() { X = -0.1, Y = 1 },
                new() { X = -0.1, Y = -1 }
            }
        };
        
        var b = new Polygon
        {
            Points = new Point[]
            {
                new() { X = -1, Y = -1 },
                new() { X = -1, Y = 1 },
                new() { X = -0.1, Y = 1 },
                new() { X = -0.1, Y = -1 }
            }
        };

        Assert.True(a.Overlaps(b));
    }
    
    [Fact]
    public void Overlap()
    {
        var polygon = new Polygon
        {
            Points = new Point[]
            {
                new() { X = 1, Y = -179 },
                new() { X = 1, Y = -166 },
                new() { X = 7, Y = -166 },
                new() { X = 7, Y = -180 }
            }
        };
        
        Assert.False(polygon.HasIntersections());
    }
}