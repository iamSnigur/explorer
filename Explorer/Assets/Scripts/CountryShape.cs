using System.Collections.Generic;

public class CountryShape
{
    public IEnumerable<Shape> Shapes => _shapes;

    private Shape[] _shapes;

    public CountryShape(Shape[] shapes)
    {
        _shapes = shapes;
    }
}
