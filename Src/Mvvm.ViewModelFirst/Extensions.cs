using System;
using System.Threading;

namespace MVVM.ViewModelFirst
{
    public static class Extensions
    {
        public static void Raise<TEventArgs>(this TEventArgs e, object sender,
            ref EventHandler<TEventArgs> eventDelegate)
        {
            EventHandler<TEventArgs> temp = Volatile.Read(ref eventDelegate);

            temp?.Invoke(sender, e);
        }

        public static void Raise(this EventArgs e, object sender, ref EventHandler eventDelegate)
        {
            EventHandler temp = Volatile.Read(ref eventDelegate);

            temp?.Invoke(sender, e);
        }
    }
}
