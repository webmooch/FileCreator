using FileCreator.Models;
using FileCreator.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            InteractiveCheckForUpdatesAsync(showMsgIfFailure: true, showMsgIfAlreadyLatestVersion: true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InteractiveCheckForUpdatesAsync(showMsgIfFailure: false, showMsgIfAlreadyLatestVersion: false);
        }

        private void InteractiveCheckForUpdatesAsync(bool showMsgIfFailure, bool showMsgIfAlreadyLatestVersion)
        {
            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork += (s, e) =>
            {
                try
                {
                    var versionData = VersionData.GetVersionData(Settings.Default.StaticVersionUri);

                    if (!versionData.IsValid)
                        throw new ArgumentException("Invalid dynamic version data.");

                    if (versionData.NewVersionAvailable)
                    {
                        var msgResult = MessageBox.Show("Congratulations - A new and improved version is available!\r\n\r\nWould you like to obtain the latest version now?", "New Version Available", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (msgResult == MessageBoxResult.Yes)
                            Process.Start(versionData.DownloadUri);
                    }
                    else if (showMsgIfAlreadyLatestVersion)
                    {
                        MessageBox.Show("You are running the latest version.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.IsCritical())
                        throw;

                    if (showMsgIfFailure)
                        MessageBox.Show(string.Format("Error determining version: {0}", ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            };
            bgWorker.RunWorkerAsync();
        }

        private void FileContextMenu_Opening(object sender, ContextMenuEventArgs e)
        {
            // This should all be bound to VM properties / use converters but this approach is significantly less code and xaml so for now it'll do
            var filePathHasError = Validation.GetHasError(textboxFile);
            var filePathExists = File.Exists(textboxFile.Text);
            menuitemOpenFile.IsEnabled = !filePathHasError && filePathExists;
            menuitemOpenFileDirectory.IsEnabled = !filePathHasError;
            menuitemDeleteFile.IsEnabled = !filePathHasError && filePathExists;
        }
    }
}