using UnityEngine;

public class Shape
{
    private Vector3[] _points;

    public Shape(Vector3[] points)
    {
        _points = points;
    }

    public void Create(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = _points.Length;
        
        for (int i = 0; i < _points.Length; i++)
            lineRenderer.SetPosition(i, _points[i]);
    }
}
