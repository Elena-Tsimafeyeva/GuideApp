using GuideApp.ViewModel;
using ZXing.Net.Maui;

namespace GuideApp;

public partial class CameraPage : ContentPage
{
    private CameraPageVM ViewModel => BindingContext as CameraPageVM;
    //Флаг, который защищает от повторного считывания, двойного открытия страницы, многократных событий ZXing
    private bool _isHandlingQr;
    public CameraPage()
    {
        InitializeComponent();
        BindingContext = new CameraPageVM();
        //Настройки сканера
        cameraView.Options = new BarcodeReaderOptions
        {
            //Сканируем только QR
            Formats = BarcodeFormat.QrCode,
            // ZXing будет автоматически поворачивать изображение, роэтому QR можно сканировать под углом
            AutoRotate = true,
            //За один кадр читается только один QR
            Multiple = false
        };
    }
    //Вызывается, когда страница появляется на экране
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //Запрашивается доступ к камере
        await ViewModel.CheckCameraPermission();

        ViewModel.CanScan = true;
        cameraView.IsDetecting = true;
    }
    //Главный обработчик QR
    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        //Если QR уже обрабатывается, то выходим
        if (_isHandlingQr)
            return;
        //Берём первый найденный QR
        var result = e.Results.FirstOrDefault();
        //Защита от null
        if (result == null)
            return;
        //Новые QR игнорируются, во избежание открытия сразу нескольких статей
        _isHandlingQr = true;

        // Остановка сканирования
        cameraView.IsDetecting = false;

        try
        {
            // переводим в UI thread сразу
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                //ProcessQrCode передаёт текст QR в ViewModel
                await ViewModel.ProcessQrCode(result.Value);
            });
        }
        finally
        {
            // Пауза, чтобы ZXing не считал QR снова
            await Task.Delay(1000);

            _isHandlingQr = false;
            cameraView.IsDetecting = true;
        }
    }

}