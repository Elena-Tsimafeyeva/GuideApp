using Mapsui;
using Mapsui.Features;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using NetTopologySuite.Geometries;

namespace GuideApp;

public partial class MapPage : ContentPage
{
    public MapPage()
    {
        InitializeComponent();
        InitMap();
        AddPoints();
        AddRoute(); // добавляем маршрут
    }

    void InitMap()
    {
        var map = new Mapsui.Map();
        map.Layers.Add(OpenStreetMap.CreateTileLayer());

        MapControl.Map = map;

        // Центр карты Гомель
        var (x, y) = SphericalMercator.FromLonLat(30.992714, 52.431212);
        map.Navigator.CenterOnAndZoomTo(new MPoint(x, y), 14);
    }

    void AddPoints()
    {
        var layer = new MemoryLayer
        {
            Name = "Points",
            Features = new IFeature[]
            {
                CreatePoint(30.991398, 52.430849, "ЖД Вокзал"),
                CreatePoint(30.993239, 52.433869, "Автовокзал")
            }
        };

        MapControl.Map.Layers.Add(layer);
    }

    void AddRoute()
    {
        // Массив точек маршрута
        var routePoints = new[]
        {
            (30.991398, 52.430849), // ЖД Вокзал
            (30.9925, 52.4315),     // промежуточная точка
            (30.993239, 52.433869)  // Автовокзал
        };

        // Конвертируем координаты в Coordinate для NetTopologySuite
        var coords = routePoints
            .Select(p =>
            {
                var (x, y) = SphericalMercator.FromLonLat(p.Item1, p.Item2);
                return new Coordinate(x, y);
            })
            .ToArray();

        // Создаём линию маршрута
        var lineFeature = new GeometryFeature
        {
            Geometry = new LineString(coords)
        };

        // Стиль линии
        lineFeature.Styles.Add(new VectorStyle
        {
            Line = new Pen(Mapsui.Styles.Color.Blue, 3)
        });

        // Слой для маршрута
        var routeLayer = new MemoryLayer
        {
            Name = "Route",
            Features = new[] { lineFeature }
        };

        MapControl.Map.Layers.Add(routeLayer);
    }

    IFeature CreatePoint(double lon, double lat, string name)
    {
        var (x, y) = SphericalMercator.FromLonLat(lon, lat);

        var feature = new GeometryFeature
        {
            Geometry = new NetTopologySuite.Geometries.Point(x, y) // Используем NetTopologySuite.Point
        };

        feature.Styles.Add(new SymbolStyle
        {
            Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.Red),
            Outline = new Pen(Mapsui.Styles.Color.Black, 2),
            SymbolScale = 0.8
        });

        feature.Styles.Add(new LabelStyle
        {
            Text = name,
            Offset = new Offset(0, -20)
        });

        return feature;
    }
}