using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras;

public partial class App : Application
{
    public static SQLiteDatabaseHelper Db { get; private set; }

    public App()
    {
        InitializeComponent();

        string path = Path.Combine(
            FileSystem.AppDataDirectory,
            "compras.db3"
        );

        Db = new SQLiteDatabaseHelper(path);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}