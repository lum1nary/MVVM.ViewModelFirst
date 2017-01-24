using System;
using System.Collections.Generic;

namespace MVMM.ViewModelFirst
{
    public sealed class ViewResolver : IViewResolver
    {
        private static readonly object StaticSyncRoot;
        private static readonly Dictionary<Type, Type> StaticAssociations;
        private static readonly Dictionary<Type, ISingletonFactoryContainer> SingletonMethodContainer;

        static ViewResolver()
        {
            StaticAssociations = new Dictionary<Type, Type>();
            SingletonMethodContainer = new Dictionary<Type, ISingletonFactoryContainer>();
            StaticSyncRoot = new object();
        }

        private readonly Dictionary<Type, Type> _associations;
        private readonly object _syncRoot;
        private readonly Type _managerProtorype;

        public ViewResolver(Type resolvePrototype)
        {
            if (!typeof(IViewManager).IsAssignableFrom(resolvePrototype))
                throw new InvalidOperationException("resolvePrototype is notimplements IViewManager");

            _managerProtorype = resolvePrototype;
            _syncRoot = new object();
            _associations = new Dictionary<Type, Type>();
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
            if(container == null)
                throw new ArgumentNullException(nameof(container));

            lock (StaticSyncRoot)
            {
                if(StaticAssociations.ContainsKey(typeof(TViewModel)))
                    throw new InvalidOperationException(typeof(TViewModel).Name + "is already registered");
                
                StaticAssociations.Add(typeof(TViewModel), typeof(TView));
                SingletonMethodContainer.Add(typeof(TViewModel), container);
            }
        }

        public IViewManager ResolveSingleton<TViewModel>(object dataContext)
        {
            lock (StaticSyncRoot)
            {
                if(!StaticAssociations.ContainsKey(typeof(TViewModel)))
                    throw new InvalidOperationException("Type is Not Registered");

                var view = Activator.CreateInstance(StaticAssociations[typeof(TViewModel)]);
                var instance = Activator.CreateInstance(_managerProtorype, view, dataContext);
                return (IViewManager) instance;
            }
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
            lock (StaticSyncRoot)
            {
                StaticAssociations.Remove(typeof(TViewModel));
                SingletonMethodContainer.Remove(typeof(TViewModel));
            }
        }

        public TViewModel GetSingletonViewModel<TViewModel>()
        {
            lock (StaticSyncRoot)
            {
                if(!SingletonMethodContainer.ContainsKey(typeof(TViewModel)))
                    throw new InvalidOperationException("Requested singleton is not registered");

                return (TViewModel)SingletonMethodContainer[typeof(TViewModel)].GetSingletonInstance();
            }
        }
    }
}
