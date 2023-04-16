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
                new() { Longitude = 0, Latitude = 0 },
                new() { Longitude = 0.5, Latitude = 1 },
                new() { Longitude = 1, Latitude = 0 }
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
                new() { Longitude = 0, Latitude = 1 },
                new() { Longitude = -1, Latitude = -3 },
                new() { Longitude = -0.5, Latitude = 1 },
                new() { Longitude = -5, Latitude = 3 },
                new() { Longitude = 3, Latitude = 5 },
                new() { Longitude = -1.5, Latitude = 2.5 },
                new() { Longitude = 5, Latitude = 3.5 },
                new() { Longitude = 2, Latitude = -7 }
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
                new() { Longitude = -1, Latitude = -1 },
                new() { Longitude = 1, Latitude = -1 },
                new() { Longitude = 1, Latitude = 1 },
                new() { Longitude = 0, Latitude = -1 },
                new() { Longitude = -1, Latitude = 1 }
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
                new() { Longitude = 0, Latitude = 0 },
                new() { Longitude = 0.5, Latitude = 0.5 },
                new() { Longitude = 2, Latitude = 2 },
                new() { Longitude = 5, Latitude = 5 }
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
                new() { Longitude = 0, Latitude = 0 },
                new() { Longitude = 0, Latitude = 1 },
                new() { Longitude = 1, Latitude = 0 },
                new() { Longitude = 1, Latitude = 1 }
            }
        };

        Assert.True(path.HasIntersections());
    }
    
    [Fact]
    public void OverlappingEdge()
    {
        var a = new Polygon
        {
            Points = new Point[]
            {
                new() { Longitude = -1, Latitude = -1 },
                new() { Longitude = -1, Latitude = 1 },
                new() { Longitude = 0, Latitude = 1 },
                new() { Longitude = 0, Latitude = -1 }
            }
        };
        
        var b = new Polygon
        {
            Points = new Point[]
            {
                new() { Longitude = 1, Latitude = 1 },
                new() { Longitude = 1, Latitude = -1 },
                new() { Longitude = 0, Latitude = -1 },
                new() { Longitude = 0, Latitude = 1 }
            }
        };

        Assert.False(a.Overlaps(b));
    }
    
    [Fact]
    public void TwoRectanglesAlmostOverlap()
    {
        var a = new Polygon
        {
            Points = new Point[]
            {
                new() { Longitude = -1, Latitude = -1 },
                new() { Longitude = -1, Latitude = 1 },
                new() { Longitude = -0.1, Latitude = 1 },
                new() { Longitude = -0.1, Latitude = -1 }
            }
        };
        
        var b = new Polygon
        {
            Points = new Point[]
            {
                new() { Longitude = 1, Latitude = 1 },
                new() { Longitude = 1, Latitude = -1 },
                new() { Longitude = 0.1, Latitude = -1 },
                new() { Longitude = 0.1, Latitude = 1 }
            }
        };

        Assert.False(a.Overlaps(b));
    }
    
    [Fact]
    public void Overlap()
    {
        var a = new Polygon
        {
            Points = new Point[]
            {
                new() { Longitude = -179, Latitude = 1 },
                new() { Longitude = -166, Latitude = 1 },
                new() { Longitude = -166, Latitude = 7 },
                new() { Longitude = -180, Latitude = 7 }
            }
        };
        
        var b = new Polygon
        {
            Points = new Point[]
            {
                new() { Longitude = -166, Latitude = 14 },
                new() { Longitude = -179, Latitude = 14 },
                new() { Longitude = -172.5, Latitude = 3 }
            }
        };
        
        Assert.True(a.Overlaps(b));
    }
}