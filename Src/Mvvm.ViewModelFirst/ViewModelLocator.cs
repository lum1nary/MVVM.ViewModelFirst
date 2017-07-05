using System;

namespace MVVM.ViewModelFirst
{
    public sealed class ViewModelLocator
    {
        private static readonly object SyncRoot = new object();
        static ViewModelLocator()
        {
            _initialized = false;
        }
        public static void Initialize(IViewResolver resolver, Action customInitialize = null)
        {
            lock (SyncRoot)
            {
                if (_initialized)
                    throw new InvalidOperationException("ViewModelLocator already Initialized");

                _viewResolver = resolver;

                customInitialize?.Invoke();

                _initialized = true;
            }
        }

        private static volatile bool _initialized;

        private static IViewResolver _viewResolver;
        public static IViewResolver ViewResolver => _viewResolver;
    }
}