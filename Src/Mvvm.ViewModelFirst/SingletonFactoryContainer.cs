using System;

namespace MVVM.ViewModelFirst
{
    public class SingletonFactoryContainer<T> : ISingletonFactoryContainer where T : class
    {
        private readonly Func<T> _getInstanceMethod;
        public SingletonFactoryContainer(Func<T> getInstanceMethod)
        {
            if(getInstanceMethod == null)
                throw new NullReferenceException("GetInstanceMethod is null");

            _getInstanceMethod = getInstanceMethod;
        }

        public object GetSingletonInstance()
        {
            return _getInstanceMethod.Invoke();
        }
    }
}
