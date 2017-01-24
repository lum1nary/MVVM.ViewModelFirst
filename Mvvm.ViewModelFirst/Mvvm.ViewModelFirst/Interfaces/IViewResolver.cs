namespace MVMM.ViewModelFirst
{
    public interface IViewResolver
    {
		//TODO RegisterSingleton and ResolveSingleton
        void Register<TViewModel, TView>();

        void RegisterSingleton<TViewModel, TView>(ISingletonFactoryContainer container);

        IViewManager Resolve<TViewModel>(object dataContext);

        IViewManager ResolveSingleton<TViewModel>();

        void Unregister<TViewModel>();

        void UnregisterSingleton<TViewModel>();
        TViewModel GetSingletonViewModel<TViewModel>();
    }
}