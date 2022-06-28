using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class DataLoader : MonoBehaviour
{
    private const string _datasetPath = "https://pkgstore.datahub.io/core/geo-countries/countries/archive/23f420f929e0e09c39d916b8aaa166fb/countries.geojson";

    [Range(0, 254)][SerializeField] private int _countryIndex;
    [Range(1, 20)][SerializeField] private float _radius;
    [SerializeField] private LineRenderer _lineRendererPrefab;

    IEnumerator Start()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_datasetPath))
        {
            yield return request.SendWebRequest();

            string json = request.downloadHandler.text;

            Debug.Log(request.result);
            Debug.Log(json);

            GeoJSON.Net.Feature.FeatureCollection featureCollection = JsonConvert.DeserializeObject<GeoJSON.Net.Feature.FeatureCollection>(json);
            Debug.Log(featureCollection.Features[_countryIndex].Properties["ADMIN"]);

            foreach(GeoJSON.Net.Feature.Feature feature in featureCollection.Features)
                CreateCountryShape(ShapeFactory.CreateShape(feature.Geometry), (string)feature.Properties["ADMIN"]);          
        };
    }

    private void CreateCountryShape(CountryShape[] countryShapes, string countryName)
    {
        foreach(CountryShape countryShape in countryShapes)
        {
            foreach(Shape shape in countryShape.Shapes)
            {
                LineRenderer lineRenderer = Instantiate(_lineRendererPrefab, transform);
                lineRenderer.name = countryName;
                shape.Create(lineRenderer);
            }
        }
    }

    private Vector3 CoordinatesToPoint(double longitude, double latitude)
    {
        /*float x = (float)(_radius * Math.Cos(latitude) * Math.Cos(longitude));
        //float x = (float)(340f + ((longitude * 340f) / 180f));
        float y = (float)(_radius * Math.Cos(latitude) * Math.Sin(longitude));
        //float y = (float)(240f - ((latitude * 240f) / 180f));
        */

        /*float r = (float)Math.Cos(latitude);
        float x = (float)Math.Sin(longitude) * r * _radius;
        float z = (float)-Math.Cos(longitude) * r * _radius;
        */

        return new Vector3((float)longitude * _radius, 0, (float)latitude * _radius);
    }
}