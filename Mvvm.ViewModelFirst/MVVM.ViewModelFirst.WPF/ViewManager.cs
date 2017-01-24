using System.Windows;

namespace MVVM.ViewModelFirst.WPF
{
    public sealed class ViewManager : ViewManagerBase
    {
        public ViewManager(Window window, object dataContext) : base(window, dataContext)
        {
        }
    }
}