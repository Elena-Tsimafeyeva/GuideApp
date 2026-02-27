using ZXing.Net.Maui;
using Microsoft.Maui.ApplicationModel;

namespace GuideApp;

public partial class CameraPage : ContentPage
{
    bool isProcessing;
    public CameraPage()
	{
		InitializeComponent();
        cameraView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlertAsync("Ошибка", "Нет доступа к камере", "OK");
            return;
        }

        cameraView.IsDetecting = true;
    }

    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (isProcessing)
            return;

        var result = e.Results.FirstOrDefault();
        if (result == null)
            return;

        isProcessing = true;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            cameraView.IsDetecting = false;

            string scannedCode = result.Value;

            await DisplayAlertAsync("QR найден", scannedCode, "OK");

            cameraView.IsDetecting = true;
            isProcessing = false;
        });
    }
}