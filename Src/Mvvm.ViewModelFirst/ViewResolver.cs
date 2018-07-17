using System;
using System.Collections.Generic;

namespace MVVM.ViewModelFirst
{
    public sealed class ViewResolver : IViewResolver
    {
        private readonly Dictionary<Type, ISingletonFactoryContainer> _singletonMethodContainer;
        private readonly Dictionary<Type, Type> _singletonAssociations = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _associations = new Dictionary<Type, Type>();
        private readonly object _syncRoot = new object();
        private readonly Type _managerProtorype;

        public ViewResolver(Type resolvePrototype)
        {
            if (!typeof(IViewManager).IsAssignableFrom(resolvePrototype))
                throw new InvalidOperationException("resolvePrototype is notimplements IViewManager");

            _managerProtorype = resolvePrototype;
        }

        public void Register<TViewModel, TView>()
        {
            lock (_syncRoot)
            {
                if (_associations.ContainsKey(typeof(TViewModel)))
                    throw new InvalidOperationException("Already Registered");

                _associations.Add(typeof(TViewModel), typeof(TView));
            }
        }

        public IViewManager Resolve<TViewModel>(object dataContext)
        {
            lock (_syncRoot)
            {
                if (!_associations.ContainsKey(typeof(TViewModel)))
                    throw new InvalidOperationException("Type is Not Registered");

                var view = Activator.CreateInstance(_associations[typeof(TViewModel)]);
                var instance = Activator.CreateInstance(_managerProtorype, view, dataContext);

                return (IViewManager)instance;
            }
        }

        public void RegisterSingleton<TViewModel, TView>(ISingletonFactoryContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (_singletonAssociations.ContainsKey(typeof(TViewModel)))
                throw new InvalidOperationException(typeof(TViewModel).Name + "is already registered");

            _singletonAssociations.Add(typeof(TViewModel), typeof(TView));
            _singletonMethodContainer.Add(typeof(TViewModel), container);
        }

        public IViewManager ResolveSingleton<TViewModel>(object dataContext)
        {
            if (!_singletonAssociations.ContainsKey(typeof(TViewModel)))
                throw new InvalidOperationException("Type is Not Registered");

            var view = Activator.CreateInstance(_singletonAssociations[typeof(TViewModel)]);
            var instance = Activator.CreateInstance(_managerProtorype, view, dataContext);
            return (IViewManager)instance;
        }

        public void Unregister<TViewModel>()
        {
            lock (_syncRoot)
            {
                _associations.Remove(typeof(TViewModel));
            }
        }

        public void UnregisterSingleton<TViewModel>()
        {
            _singletonAssociations.Remove(typeof(TViewModel));
            _singletonMethodContainer.Remove(typeof(TViewModel));
        }

        public TViewModel GetSingletonViewModel<TViewModel>()
        {
            if (!_singletonMethodContainer.ContainsKey(typeof(TViewModel)))
                throw new InvalidOperationException("Requested singleton is not registered");

            return (TViewModel)_singletonMethodContainer[typeof(TViewModel)].GetSingletonInstance();
        }
    }
}
