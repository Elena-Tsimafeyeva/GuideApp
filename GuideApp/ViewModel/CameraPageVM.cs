using GuideApp.DB;


namespace GuideApp.ViewModel
{
    public class CameraPageVM : ViewModelBase
    {
        private DatabaseService _database = new DatabaseService();
        //Вторая защита от повторной обработки
        private bool isProcessing;
        //Свойство для Binding. Через него можно: отключать UI, показывать индикатор, скрывать камеру
        private bool canScan = true;
        public bool CanScan
        {
            get => canScan;
            set
            {
                canScan = value;
                OnPropertyChanged();
            }
        }
        //Хранит последний QR
        private string scannedText;
        public string ScannedText
        {
            get => scannedText;
            set
            {
                scannedText = value;
                OnPropertyChanged();
            }
        }

        // Проверка разрешения камеры
        public async Task CheckCameraPermission()
        {
            //Проверка разрешения. Проверяет разрешена камера или нет
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            //Запрос разрешения. Если доступа нет, то показывается системное окно
            if (status != PermissionStatus.Granted)
            {
                                status = await Permissions.RequestAsync<Permissions.Camera>();
            }
            //Если пользователь отказал, то показывается ошибка
            if (status != PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Нет доступа к камере", "OK");

                CanScan = false;
                return;
            }
            //Запрещаем сканирование
            CanScan = true;
        }
        //Главная бизнес-логика
        public async Task ProcessQrCode(string qrText)
        {
            //Повторная защита
            if (isProcessing)
                return;
            //Блокировка
            isProcessing = true;
            CanScan = false;

            try
            {
                // Сохраняем считанный QR
                ScannedText = qrText;

                // Ищем статью по QR_Code
                var article = _database.GetArticleByQr(qrText);

                // Если статья не найдена
                if (article == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Статья не найдена", "OK");

                    return;
                }

                // Создаём ViewModel статьи
                var articleVM = new ArticlePageVM();

                // Загружаем данные статьи
                articleVM.LoadArticle(article.IdArticle, article.Title);

                // Переходим на страницу статьи
                await Application.Current.MainPage.Navigation.PushAsync(
                    new ArticlePage
                    {
                        BindingContext = articleVM
                    });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "OK");
            }
            finally
            {
                //Сбрасываем блокировки
                CanScan = true;
                isProcessing = false;
            }
        }

    }
}
