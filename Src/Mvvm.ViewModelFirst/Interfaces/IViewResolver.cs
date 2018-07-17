namespace MVVM.ViewModelFirst
{
    public interface IViewResolver
    {
        void Register<TViewModel, TView>();

        void RegisterSingleton<TViewModel, TView>(ISingletonFactoryContainer container);

        IViewManager Resolve<TViewModel>(object dataContext);

        IViewManager ResolveSingleton<TViewModel>(object dataContext);

        void Unregister<TViewModel>();

        void UnregisterSingleton<TViewModel>();
        TViewModel GetSingletonViewModel<TViewModel>();
    }
}