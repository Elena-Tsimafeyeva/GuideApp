using BruTile.MbTiles;
using Mapsui;
using Mapsui.Features;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using Mapsui.UI.Maui;
using NetTopologySuite.Geometries;
using SQLite;

namespace GuideApp;

public partial class MapPage : ContentPage
{
    public MapPage()
    {
        InitializeComponent();
        InitMap();
        //AddRoute(); // добавляем маршрут
    }

    async void InitMap()
    {
        var map = new Mapsui.Map();

        var file = await FileSystem.OpenAppPackageFileAsync("map.mbtiles");
        var path = Path.Combine(FileSystem.CacheDirectory, "map.mbtiles");

        using (var fs = File.Create(path))
        {
            await file.CopyToAsync(fs);
        }

        var sqlite = new SQLiteConnectionString(path, true);
        var tileSource = new MbTilesTileSource(sqlite);
        var tileLayer = new TileLayer(tileSource);

        map.Layers.Add(tileLayer);

        MapControl.Map = map;
        AddPoints();
        //это прошлый код, который я использовала для онлайн карты
        //var (x, y) = SphericalMercator.FromLonLat(52.337841, 30.964029);
        //map.Navigator.CenterOnAndZoomTo(new MPoint(x, y), 14);
        //var map = new Mapsui.Map();
        //map.Layers.Add(OpenStreetMap.CreateTileLayer());

        //MapControl.Map = map;

        // Центр карты Гомель
        //var (x, y) = SphericalMercator.FromLonLat(30.992714, 52.431212);
        //map.Navigator.CenterOnAndZoomTo(new MPoint(x, y), 14);
    }

    void AddPoints()
    {
        var layer = new MemoryLayer
        {
            Name = "Points",
            Features = new IFeature[]
            {
                CreatePoint(30.964415, 52.344798, "Точка 1. История поселка Чёнки"),
                CreatePoint(30.965268, 52.345072, "Точка 2 Река Сож"),
                CreatePoint(30.956294195989894, 52.33682083021225, "Точка 3 Санаторий «Машиностроитель»"),
                CreatePoint(30.954658, 52.331347, "Точка 4 Озеро Узкое"),
                CreatePoint(30.952301, 52.328403, "Точка 5. Пойменный ландшафт"),
                CreatePoint(30.961000, 52.328056, "Точка 6 Ченковский лес"),
                CreatePoint(30.960556, 52.330444, "Точка 7 Древнее городище")
            }
        };

        MapControl.Map.Layers.Add(layer);
    }
    //это прошлый код, который я использовала для маршрутов
    //void AddRoute()
    //{
    //    // Массив точек маршрута
    //    var routePoints = new[]
    //    {
    //        (30.991398, 52.430849), // ЖД Вокзал
    //        (30.9925, 52.4315),     // промежуточная точка
    //        (30.993239, 52.433869)  // Автовокзал
    //    };

    //    // Конвертируем координаты в Coordinate для NetTopologySuite
    //    var coords = routePoints
    //        .Select(p =>
    //        {
    //            var (x, y) = SphericalMercator.FromLonLat(p.Item1, p.Item2);
    //            return new Coordinate(x, y);
    //        })
    //        .ToArray();

    //    // Создаём линию маршрута
    //    var lineFeature = new GeometryFeature
    //    {
    //        Geometry = new LineString(coords)
    //    };

    //    // Стиль линии
    //    lineFeature.Styles.Add(new VectorStyle
    //    {
    //        Line = new Pen(Mapsui.Styles.Color.Blue, 3)
    //    });

    //    // Слой для маршрута
    //    var routeLayer = new MemoryLayer
    //    {
    //        Name = "Route",
    //        Features = new[] { lineFeature }
    //    };

    //    MapControl.Map.Layers.Add(routeLayer);
    //}

    IFeature CreatePoint(double lon, double lat, string name)
    {
        var (x, y) = SphericalMercator.FromLonLat(lon, lat);

        var feature = new GeometryFeature
        {
            Geometry = new NetTopologySuite.Geometries.Point(x, y)
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