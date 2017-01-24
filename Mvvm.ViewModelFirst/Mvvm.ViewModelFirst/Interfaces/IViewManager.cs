using System;

namespace MVMM.ViewModelFirst
{
    public interface IViewManager
    {
        void ShowMessageBox(string message, string caption, bool shouldClose = false);

        bool ShowYesNoMessageBox(string message, string caption);

        bool? ShowYesNoCancelMessageBox(string message, string caption);

        bool IsShowing { get; }

        void Hide();

        void Show();

        void Close();

        bool ShowSaveFileDialog(string caption, string filter, out string filepath);

        bool ShowOpenFileDIalog(string caption, string filter, out string filepath);

        bool ShowMultiOpenFileDialog(string caption, string filter, out string[] filepathes);

        event EventHandler WindowClosed;

    }
}