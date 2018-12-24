using FreshMvvm;

namespace HttpDemo
{
    public static class Container
    {
        public static void Register()
        {
            var container = FreshIOC.Container;
            container.Register<ISingletonApiService, SingletonApiService>().AsSingleton();
            container.Register<IDisposableApiService, DisposableApiService>().AsSingleton();
        }
    }
}