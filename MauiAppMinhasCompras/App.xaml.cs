using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQliteDatabaseHelper _db;
        public static SQliteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "banco_sqlite_compras.db3");
                    _db = new SQliteDatabaseHelper(path);

                }
                return _db;
            }
        }
        public App()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            //return new Window(new AppShell());
            MainPage = new NavigationPage(new Views.ListaProduto());
        }

       
    }
}