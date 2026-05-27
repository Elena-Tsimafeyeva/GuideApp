using BruTile.MbTiles;
using GuideApp.ViewModel;
using Mapsui.Layers;
using Mapsui.Tiling.Layers;
using Mapsui.UI.Maui;
using SQLite;

namespace GuideApp;

public partial class MapPage : ContentPage
{
    //Главный объект карты, который хранит: слои, тайлы, маршруты, точки
    private Mapsui.Map _map;
    private MapPageVM ViewModel => BindingContext as MapPageVM;
    public MapPage()
    {
        InitializeComponent();
        var vm = new MapPageVM();
        BindingContext = vm;
        //Подписка на события
        vm.OnLayerChanged += UpdateLayers;
        vm.OnMyLocation += UpdateMyLocation;
        vm.OnLocationWarning += ShowWarning;
        //Главная загрузка карты
        InitMap();
    }
    //Создание карты
    private async void InitMap()
    {
        //Создаётся объект Mapsui
        _map = new Mapsui.Map();
        //Открывается файл карты из ресурсов приложения
        var file = await FileSystem.OpenAppPackageFileAsync("map.mbtiles");
        //Копируем в Cache
        var path = Path.Combine(FileSystem.CacheDirectory, "map.mbtiles");
        //Карта копируется в кэш устройства
        using (var fs = File.Create(path))
            await file.CopyToAsync(fs);
        //Mapsui читает MBTiles как SQLite базу (.mbtiles = SQLite database)
        var sqlite = new SQLiteConnectionString(path, true);
        //Источник тайлов карты
        var tileSource = new MbTilesTileSource(sqlite);
        //Добавляется слой самой карты
        _map.Layers.Add(new TileLayer(tileSource));
        //Привязка карты к UI-контролу
        MapControl.Map = _map;
    }
    //Получает слой маршрута и слой точек из VM
    private void UpdateLayers(MemoryLayer route, MemoryLayer points)
    {
        //Удаляет старые слои, чтобы маршруты не накладывались друг на друга
        RemoveLayer("RouteLayer");
        RemoveLayer("PointsLayer");

        // Сначала рисуем линию
        _map.Layers.Add(route);

        // Потом рисуем точки (они будут сверху)
        _map.Layers.Add(points);
        //Перерисовка карты
        MapControl.Refresh();
    }
    //Обновляет слой "Вы тут"
    private void UpdateMyLocation(MemoryLayer layer)
    {
        //Удаляет старую позицию
        RemoveLayer("MyLocationLayer");

        _map.Layers.Add(layer);
        MapControl.Refresh();
    }

    private void ShowWarning(string message)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlert("Геолокация", message, "OK");
        });
    }
    private void RemoveLayer(string name)
    {
        //Поиск слоя по имени
        var layer = _map.Layers.FirstOrDefault(x => x.Name == name);
        if (layer != null)
            _map.Layers.Remove(layer);
    }
    

    //void AddPoints()
    //{
    //    var layer = new MemoryLayer
    //    {
    //        Name = "Points",
    //        Features = new IFeature[]
    //        {
    //            CreatePoint(30.964415, 52.344798, "Точка 1. История поселка Чёнки"),
    //            CreatePoint(30.965268, 52.345072, "Точка 2 Река Сож"),
    //            CreatePoint(30.956294195989894, 52.33682083021225, "Точка 3 Санаторий «Машиностроитель»"),
    //            CreatePoint(30.954658, 52.331347, "Точка 4 Озеро Узкое"),
    //            CreatePoint(30.952301, 52.328403, "Точка 5. Пойменный ландшафт"),
    //            CreatePoint(30.961000, 52.328056, "Точка 6 Ченковский лес"),
    //            CreatePoint(30.960556, 52.330444, "Точка 7 Древнее городище")
    //        }
    //    };

    //    MapControl.Map.Layers.Add(layer);
    //}
   
}