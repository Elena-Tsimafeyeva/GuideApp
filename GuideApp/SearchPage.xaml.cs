using GuideApp.ViewModel;
using System.Reflection;
namespace GuideApp;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
        BindingContext = new SearchPageVM();
    }
    //этот код я использую для обновления БД
    //private async void OnUpdateDatabaseClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Локальный путь для базы
    //        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Articles.db");

    //        var assembly = Assembly.GetExecutingAssembly();
    //        var resourceName = "GuideApp.Resources.Raw.Articles.db"; // используем имя EmbeddedResource, а не путь на диске

    //        using var stream = assembly.GetManifestResourceStream(resourceName);
    //        if (stream == null)
    //        {
    //            await DisplayAlert("Ошибка", "Файл базы данных не найден в ресурсах. Проверьте Build Action и имя.", "OK");
    //            return;
    //        }

    //        using var fileStream = new FileStream(dbPath, FileMode.Create, FileAccess.Write);
    //        stream.CopyTo(fileStream);

    //        await DisplayAlert("Готово", "База данных успешно обновлена!", "OK");
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Ошибка", $"Не удалось обновить базу: {ex.Message}", "OK");
    //    }
    //}
    }