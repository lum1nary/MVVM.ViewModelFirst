using System;
using System.Windows;
using System.Windows.Data;
using Microsoft.Win32;

namespace MVVM.ViewModelFirst.WPF
{
    public abstract class ViewManagerBase : IViewManager
    {
        public Window Window { get; }

        protected ViewManagerBase(Window window, object dataContext)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));

            Window = window;

            Window.Closed += (sender, args) => OnClosed(args);

            var binding = new Binding
            {
                Path = new PropertyPath(string.Empty),
                Source = dataContext,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(Window, FrameworkElement.DataContextProperty, binding);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            e.Raise(this, ref WindowClosed);
        }

        public virtual void Hide()
        {
            if (_isShowing)
            {
                _isShowing = false;
                Window?.Hide();
            }
        }

        public virtual void Show()
        {
            if (!_isShowing)
            {
                _isShowing = true;
                Window?.Show();
            }

        }

        public virtual bool ShowSaveFileDialog(string caption, string filter, out string filepath)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Title = caption;
            dialog.Filter = filter;

            if (dialog.ShowDialog() == true)
            {
                filepath = dialog.FileName;

                return true;
            }
            else
            {
                filepath = string.Empty;
                return false;
            }
        }

        public event EventHandler WindowClosed;

        public virtual void Close()
        {
            _isShowing = false;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => Window.Close()));

           EventArgs.Empty.Raise(this, ref WindowClosed);
        }


        public virtual void ShowMessageBox(string message, string caption, bool shouldClose = false)
        {
            MessageBox.Show(Window, message, caption);
            if (shouldClose)
            {
                Window.Close();
            }
        }

        public virtual bool ShowYesNoMessageBox(string message, string caption)
        {
            return MessageBox.Show(Window, message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public virtual bool? ShowYesNoCancelMessageBox(string message, string caption)
        {
            var result = MessageBox.Show(Window, message, caption, MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
                return true;
            if (result == MessageBoxResult.No)
                return false;

            return null;
        }

        private volatile bool _isShowing;
        public bool IsShowing => _isShowing;
        public virtual bool ShowOpenFileDIalog(string caption, string filter, out string filepath)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = caption,
                Filter = filter
            };

            if (dialog.ShowDialog(Window) == true)
            {
                filepath = dialog.FileName;
                return true;
            }
            else
            {
                filepath = string.Empty;
                return false;
            }

        }

        public virtual bool ShowMultiOpenFileDialog(string caption, string filter, out string[] filepathes)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = caption,
                Filter = filter,
                Multiselect = true
            };

            if (dialog.ShowDialog(Window) == true)
            {
                filepathes = dialog.FileNames;
                return true;
            }
            else
            {
                filepathes = new string[] { };
                return false;
            }
        }

        public virtual bool? ShowDialog()
        {
           return Window.ShowDialog();
        }
    }
}