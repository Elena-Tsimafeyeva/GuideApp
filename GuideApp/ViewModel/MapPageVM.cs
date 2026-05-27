using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using NetTopologySuite.Geometries;

namespace GuideApp.ViewModel
{
    public class MapPageVM : ViewModelBase
    {
        public Command GetMyLocationCommand { get; }
        public Action<MemoryLayer> OnMyLocation;

        public Action<string> OnLocationWarning;

        private CancellationTokenSource _locationCts;
        private bool _warningShown;
        
        //Событие передаёт из VM в View маршрут и точки
        public Action<MemoryLayer, MemoryLayer> OnLayerChanged;
        public Command LoadWay1Command { get; }
        public Command LoadWay2Command { get; }
        public MapPageVM()
        {
            LoadWay1Command = new Command(LoadWay1);
            LoadWay2Command = new Command(LoadWay2);
            GetMyLocationCommand = new Command(async () => await StartTrackingLocation());
        }
        //Загрузка маршрута 1
        private void LoadWay1()
        {
            //Создаются точки и маршрут
            var points = CreateWay1Points();
            var line = CreateWay1Route();
            //View получает их и рисует
            OnLayerChanged?.Invoke(line, points);
        }
        //Загрузка маршрута 2
        private void LoadWay2()
        {
            //Создаются точки и маршрут
            var points = CreateWay2Points();
            var line = CreateWay2Route();
            //View получает их и рисует
            OnLayerChanged?.Invoke(line, points);
        }

        //Points
        //Создание слоя с точками для маршрута 1
        private MemoryLayer CreateWay1Points()
        {
            return new MemoryLayer
            {
                Name = "PointsLayer",
                Features = new[]
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
        }
        //Создание слоя с точками для маршрута 2
        private MemoryLayer CreateWay2Points()
        {
            //return new MemoryLayer
            //{
            //    Name = "PointsLayer",
            //    Features = new[]
            //    {
            //     CreatePoint(30.852980,52.375932, "Точка 1"),
            //     CreatePoint(30.85099197921736,52.27202816521982, "Точка 2"),
            //     CreatePoint(31.022730239585506,52.27186893099239, "Точка 3"),
            //     CreatePoint(31.021899,52.374598, "Точка 4")
            //}
            //};
            return CreateWay1Points(); // Пока одинаково, т.к. нет координат маршрута 2
        }

        //Route
        //Создание слоя с линией маршрута 1
        private MemoryLayer CreateWay1Route()
        {
            return CreateRouteLayer(new[]
            {
                (30.964415, 52.344798), //1
                (30.965268, 52.345072), //2
                (30.965577628248457, 52.34433525519254),
                (30.959270181571114, 52.34020165758793),
                (30.958644680549533, 52.33931756717475),
                (30.958425331732453, 52.33833833582985),
                (30.956946794947967, 52.33658359695426),
                (30.956294195989894, 52.33682083021225), //3
                (30.956946794947967, 52.33658359695426),
                (30.956694521860115, 52.33629441823793),
                (30.95631902214543, 52.33495399048469),
                (30.956266588682546, 52.33412617595292),
                (30.956020656514394,52.33316366362743),
                (30.95606998838407, 52.3329057822193),
                (30.95593323814489, 52.33280549972613),
                (30.954878697816227, 52.33128815863862),
                (30.954658, 52.331347), //4
                (30.954878697816227, 52.33128815863862),
                (30.954620306860907, 52.33092328803049),
                (30.952856505317303, 52.329237870745125),
                (30.95253851791491, 52.32887509918054),
                (30.952359369360614, 52.32839816201987),
                (30.952301, 52.328403), //5
                (30.952359369360614, 52.32839816201987),
                (30.952298876441915, 52.32818383130541),
                (30.95264896759023, 52.32734912677034),
                (30.96101440680544, 52.32784278012123),
                (30.961000, 52.328056), //6
                (30.960556, 52.330444), //7
                (30.960222391559743, 52.332972019018165),
                (30.960025070435023, 52.33330874367168),
                (30.96012416197059, 52.33351840616756),
                (30.959391781401127, 52.33835315200301),
                (30.95927617176416, 52.340211799864285)
        });
        }
        //Создание слоя с линией маршрута 2
        private MemoryLayer CreateWay2Route()
        {
            //    return CreateRouteLayer(new[]
            //    {
            //    (52.375932, 30.852980),
            //        (52.27202816521982, 30.85099197921736),
            //        (52.27186893099239, 31.022730239585506),
            //        (52.374598, 31.021899)
            //});
            return CreateWay1Route();
        }
        //Метод для создания линии маршрута
        private MemoryLayer CreateRouteLayer((double lon, double lat)[] routePoints)
        {
            var coords = routePoints
                .Select(p =>
                {
                    //Конвертация координат
                    var (x, y) = SphericalMercator.FromLonLat(p.lon, p.lat);
                    return new Coordinate(x, y);
                })
                .ToArray();
            //Создаётся геометрия линии
            var line = new GeometryFeature
            {
                Geometry = new LineString(coords)
            };
            //Синяя линия толщиной 4
            line.Styles.Add(new VectorStyle
            {
                Line = new Pen(Mapsui.Styles.Color.Blue, 4)
            });

            return new MemoryLayer
            {
                Name = "RouteLayer",
                Features = new[] { line }
            };
        }

        //Point
        //Метод для создания точек
        private IFeature CreatePoint(double lon, double lat, string name)
        {
            var (x, y) = SphericalMercator.FromLonLat(lon, lat);

            var feature = new GeometryFeature
            {
                Geometry = new NetTopologySuite.Geometries.Point(x, y)
            };
            //Создаём зелёгый круг с чёрной обводкой
            feature.Styles.Add(new SymbolStyle
            {
                Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.DarkGreen),
                Outline = new Pen(Mapsui.Styles.Color.Black, 2),
                SymbolScale = 0.8
            });
            //Подпись возле точки
            feature.Styles.Add(new LabelStyle
            {
                Text = name,
                //Текст рисуется выше точки
                Offset = new Offset(0, -20)
            });

            return feature;
        }

        // Метод StartTrackingLocation запускает бесконечное GPS-отслеживание
        private async Task StartTrackingLocation()
        {
            //Останавливает старое отслеживание
            _locationCts?.Cancel();
            //Создание нового токена. Теперь можно отменить цикл позже
            _locationCts = new CancellationTokenSource();

            try
            {
                var request = new GeolocationRequest(
                    //Высокая точность GPS
                    GeolocationAccuracy.High,
                    TimeSpan.FromSeconds(10));
                //Бесконечный цикл обновления GPS
                while (!_locationCts.Token.IsCancellationRequested)
                {
                    //MAUI получает координаты устройства
                    var location = await Geolocation.Default.GetLocationAsync(request);

                    if (_locationCts.Token.IsCancellationRequested)
                        break;
                    //Проверка null
                    if (location == null)
                    {
                        OnLocationWarning?.Invoke("Не удалось получить геолокацию");
                        await Task.Delay(3000);
                        continue;
                    }
                    //Проверка находится ли пользователь внутри карты
                    if (!IsInsideMapArea(location.Latitude, location.Longitude))
                    {
                        if (!_warningShown)
                        {
                            _warningShown = true;
                            //Если пользователь вне карты
                            OnLocationWarning?.Invoke("Вы не на местности (за пределами карты)");
                        }

                        await Task.Delay(3000);

                        if (_locationCts.Token.IsCancellationRequested)
                            break;

                        continue;
                    }
                    else
                    {
                        //Защита от спама alert-окнами
                        _warningShown = false;
                    }
                    //Создаёт красную точку "Вы тут"
                    var layer = CreateMyLocationLayer(location.Longitude, location.Latitude);
                    OnMyLocation?.Invoke(layer);

                    await Task.Delay(2000);

                    if (_locationCts.Token.IsCancellationRequested)
                        break;
                }
            }
            catch (Exception ex)
            {
                OnLocationWarning?.Invoke(ex.Message);
            }
        }

        // Проверка выхода на карту
        private bool IsInsideMapArea(double lat, double lon)
        {
            //Координаты 4-х углов карты (границ)
            var polygon = new (double lat, double lon)[]
            {
                (52.375932, 30.852980),
                (52.27202816521982, 30.85099197921736),
                (52.27186893099239, 31.022730239585506),
                (52.374598, 31.021899)
            };

            int j = polygon.Length - 1;
            bool inside = false;

            for (int i = 0; i < polygon.Length; i++)
            {
                var xi = polygon[i].lon;
                var yi = polygon[i].lat;

                var xj = polygon[j].lon;
                var yj = polygon[j].lat;

                bool intersect = ((yi > lat) != (yj > lat)) &&
                                 (lon < (xj - xi) * (lat - yi) / (yj - yi + 0.0000001) + xi);

                if (intersect)
                    inside = !inside;

                j = i;
            }

            return inside;
        }
        // Точка "Вы тут"
        private MemoryLayer CreateMyLocationLayer(double lon, double lat)
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
                SymbolScale = 1.2
            });

            feature.Styles.Add(new LabelStyle
            {
                Text = "Вы тут",
                Offset = new Offset(0, -15)
            });

            return new MemoryLayer
            {
                Name = "MyLocationLayer",
                Features = new[] { feature }
            };
        }
    }
}
