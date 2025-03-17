namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //return new Window(new AppShell());
            MainPage = new NavigationPage(new Views.ListaProduto());
        }

       
    }
}