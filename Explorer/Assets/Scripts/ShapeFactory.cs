using System;
using System.Collections.Generic;
using UnityEngine;

public static class ShapeFactory
{
    public static CountryShape[] CreateShape(GeoJSON.Net.Geometry.IGeometryObject geometryObject)
    {
        switch(geometryObject.Type)
        {
            case GeoJSON.Net.GeoJSONObjectType.Polygon:
                return CreatePolygon(geometryObject);
            case GeoJSON.Net.GeoJSONObjectType.MultiPolygon:
                return CreateMultiPolygon(geometryObject);
                default: throw new ArgumentException($"No supported IGeometryObject of type {geometryObject.GetType()}");
        }
    }

    private static CountryShape[] CreatePolygon(GeoJSON.Net.Geometry.IGeometryObject geometryObject)
    {
        GeoJSON.Net.Geometry.Polygon polygon = (GeoJSON.Net.Geometry.Polygon)geometryObject;
        CountryShape[] countryShapes = new CountryShape[1];
        countryShapes[0] = CreateCountryShape(polygon);

        return countryShapes;
    }

    private static CountryShape[] CreateMultiPolygon(GeoJSON.Net.Geometry.IGeometryObject geometryObject)
    {
        GeoJSON.Net.Geometry.MultiPolygon multiPolygon = (GeoJSON.Net.Geometry.MultiPolygon)geometryObject;
        CountryShape[] countryShapes = new CountryShape[multiPolygon.Coordinates.Count];

        for(int i = 0; i < countryShapes.Length; i++)
            countryShapes[i] = CreateCountryShape(multiPolygon.Coordinates[i]);

        return countryShapes;
    }

    private static CountryShape CreateCountryShape(GeoJSON.Net.Geometry.Polygon polygon)
    {
        Shape[] shapes = new Shape[polygon.Coordinates.Count];

        for (int i = 0; i < polygon.Coordinates.Count; i++)
        {
            GeoJSON.Net.Geometry.LineString lineString = polygon.Coordinates[i];
            Vector3[] points = new Vector3[lineString.Coordinates.Count];

            for (int j = 0; j < lineString.Coordinates.Count; j++)
            {
                points[j] = CoordinatesToPoint(lineString.Coordinates[j].Longitude, lineString.Coordinates[j].Latitude);
            }

            shapes[i] = new Shape(points);
        }
        
        return new CountryShape(shapes);
    }

    private static Vector3 CoordinatesToPoint(double longitude, double latitude)
    {
        return new Vector3((float)longitude, 0, (float)latitude);
    }
}
