using FreshMvvm;
using HttpDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HttpDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FreshPageModelResolver.PageModelMapper = new CustomPageMapper();
            Container.Register();
            var page = FreshPageModelResolver.ResolvePageModel<MainViewModel>();
            MainPage = new FreshNavigationContainer(page);
        }
    }
}
