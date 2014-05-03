using FileCreator.Helpers;
using FileCreator.Models;
using FileCreator.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace FileCreator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        // TODO: Add aggregate error handling

        private static readonly int MaxIntArrayLengthx64 = int.MaxValue - 56;

        public long TotalFileSizeBytes { get; private set; }
        public int TotalChunkSizeBytes { get; private set; }
        public List<string> PropertiesWithErrors { get; private set; }
        private CancellationTokenSource cancellationTokenSource { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        [Required(ErrorMessage = "File path is required.")]
        [StringLength(1000, ErrorMessage = "File name and path cannot exceed 1000 characters.")]
        [IsValidFullFileName()]
        public string FilePath
        {
            get { return Settings.Default.FilePath; }
            set
            {
                Settings.Default.FilePath = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "File size unit is required.")]
        public Size FileSizeUnits
        {
            get { return Settings.Default.FileSizeUnit; }
            set
            {
                Settings.Default.FileSizeUnit = value;
                UpdateTotalFileSize();
                OnPropertyChanged(""); // Must update file size so it can be rechecked for validity against the newly selected units.
            }
        }

        [Required(ErrorMessage = "File size is required.")]
        [RegularExpressionAttribute(@"\d+", ErrorMessage = "File size must be positive and consist only of numerals.")]
        [IsValidLongBytesBasedOnSizeUnit("FileSizeUnits")]
        public string FileSize
        {
            get { return Settings.Default.FileSize; }
            set
            {
                Settings.Default.FileSize = value;
                UpdateTotalFileSize();
                OnPropertyChanged("");
            }
        }

        [Required(ErrorMessage = "Chunk size is required.")]
        [RegularExpressionAttribute(@"\d+", ErrorMessage = "File size must be positive and consist only of numerals.")]
        [IsValidIntBytesBasedOnSizeUnit("ChunkSizeUnits")]
        [IntPropertyCannotBeGreaterThanLongProperty("TotalChunkSizeBytes", "TotalFileSizeBytes", ErrorMessage = "Chunk size cannot be greater than file size.")]
        public string ChunkSize
        {
            get { return Settings.Default.ChunkSize; }
            set
            {
                Settings.Default.ChunkSize = value;
                UpdateTotalChunkSize();
                OnPropertyChanged("");
            }
        }

        [Required(ErrorMessage = "Chunk unit size is required.")]
        public Size ChunkSizeUnits
        {
            get { return Settings.Default.ChunkSizeUnit; }
            set
            {
                Settings.Default.ChunkSizeUnit = value;
                UpdateTotalChunkSize();
                OnPropertyChanged("");
            }
        }

        [Required(ErrorMessage = "Data chunk type is required.")]
        public ChunkData ChunkDataType
        {
            get { return Settings.Default.ChunkDataType; }
            set
            {
                Settings.Default.ChunkDataType = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            private set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _operationInProgress;
        public bool OperationInProgress
        {
            get { return _operationInProgress; }
            private set
            {
                _operationInProgress = value;
                OnPropertyChanged();
            }
        }

        private ICommand _createAndCancelCommand;
        public ICommand CreateAndCancelCommand
        {
            get { return _createAndCancelCommand; }
            private set
            {
                _createAndCancelCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _visitProjectHomepageCommand;
        public ICommand VisitProjectHomepageCommand
        {
            get { return _visitProjectHomepageCommand; }
            private set
            {
                _visitProjectHomepageCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _restoreDefaultValuesCommand;
        public ICommand RestoreDefaultValuesCommand
        {
            get { return _restoreDefaultValuesCommand; }
            private set
            {
                _restoreDefaultValuesCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _promptForFilePathAndSaveCommand;
        public ICommand PromptForFilePathAndSaveCommand
        {
            get { return _promptForFilePathAndSaveCommand; }
            private set
            {
                _promptForFilePathAndSaveCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _openFileDirectoryCommand;
        public ICommand OpenFileDirectoryCommand
        {
            get { return _openFileDirectoryCommand; }
            private set
            {
                _openFileDirectoryCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _deleteFileCommand;
        public ICommand DeleteFileCommand
        {
            get { return _deleteFileCommand; }
            private set
            {
                _deleteFileCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _openFileCommand; }
            private set
            {
                _openFileCommand = value;
                OnPropertyChanged();
            }
        }

        private int _progressPercent = 0;
        public int ProgressPercent
        {
            get { return _progressPercent; }
            private set
            {
                _progressPercent = value;
                OnPropertyChanged();
            }
        }

        public string this[string propertyName]
        {
            get
            {
                var Results = new List<ValidationResult>();
                var isValid = Validator.TryValidateProperty(this.GetPropertyValue(propertyName), new ValidationContext(this, null, null) { MemberName = propertyName }, Results);
                if (!isValid)
                {
                    if (!PropertiesWithErrors.Contains(propertyName))
                    {
                        PropertiesWithErrors.Add(propertyName);
                        CreateAndCancelCommand.CanExecute(null);
                    }

                    var errors = Results.Select(x => x.ErrorMessage).ToArray();
                    return string.Join(Environment.NewLine, errors);
                }
                else
                {
                    if (PropertiesWithErrors.Contains(propertyName))
                    {
                        PropertiesWithErrors.Remove(propertyName);
                        CreateAndCancelCommand.CanExecute(null);
                    }

                    return string.Empty;
                }
            }
        }

        public MainViewModel()
        {
            cancellationTokenSource = new CancellationTokenSource();
            UpdateTotalFileSize();
            UpdateTotalChunkSize();
            PropertiesWithErrors = new List<string>();
            VisitProjectHomepageCommand = new Helpers.DelegateCommand(param => VisitProjectHomepage(), param => true);
            CreateAndCancelCommand = new DelegateCommand(param => CreateAndCancelButtonPressed(), param => PropertiesWithErrors.Count == 0);
            RestoreDefaultValuesCommand = new Helpers.DelegateCommand(param => RestoreDefaultValues(), param => true);
            PromptForFilePathAndSaveCommand = new Helpers.DelegateCommand(param => PromptForFilePathAndSave(), param => true);
            OpenFileDirectoryCommand = new Helpers.DelegateCommand(param => OpenFileDirectory(), param => true);
            DeleteFileCommand = new Helpers.DelegateCommand(param => DeleteFile(), param => true);
            OpenFileCommand = new Helpers.DelegateCommand(param => OpenFile(), param => true);
            CreateAndCancelCommand.CanExecuteChanged += (s, e) =>
                {
                    if (PropertiesWithErrors.Count > 0)
                        StatusMessage = "Input error detected.";
                    else
                        StatusMessage = string.Empty;
                };
        }

        private void OpenFile()
        {
            try
            {
                Process.Start(FilePath);
            }
            catch (Exception ex)
            {
                if (!ex.IsCritical())
                    MessageBox.Show(string.Format("Error opening file: {0}", ex.Message), null, MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    throw;
            }
        }

        private void DeleteFile()
        {
            try
            {
                File.Delete(FilePath);
            }
            catch (Exception ex)
            {
                if (!ex.IsCritical())
                    MessageBox.Show(string.Format("Error deleting file: {0}", ex.Message), null, MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    throw;
            }
        }

        private void OpenFileDirectory()
        {
            try
            {
                var fi = new FileInfo(FilePath);
                if (fi.Exists)
                    fi.OpenDirectory();
                else
                    Process.Start(fi.DirectoryName);
            }
            catch (Exception ex)
            {
                if (!ex.IsCritical())
                    MessageBox.Show(string.Format("Error opening file directory: {0}", ex.Message), null, MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    throw;
            }
        }

        private void PromptForFilePathAndSave()
        {
            FilePath = PromptForFilePath();
            OnPropertyChanged("");
        }

        private string PromptForFilePath()
        {
            var dialog = new SaveFileDialog();
            dialog.CheckPathExists = true;
            dialog.ValidateNames = true;
            dialog.Title = "Name and location of file to create";
            var response = dialog.ShowDialog();
            return response != null && (bool)response ? dialog.FileName : null;
        }

        private void RestoreDefaultValues()
        {
            Settings.Default.Reset();
            Settings.Default.Reload();
            OnPropertyChanged("");
        }

        private void UpdateTotalFileSize()
        {
            long value;
            if (long.TryParse(FileSize, out value))
            {
                switch (FileSizeUnits)
                {
                    case Size.B:
                        TotalFileSizeBytes = value;
                        break;

                    case Size.KB:
                        TotalFileSizeBytes = (value * 1024 > long.MaxValue) ? long.MaxValue : value * 1024;
                        break;

                    case Size.MB:
                        TotalFileSizeBytes = (value * 1024 * 1024 > long.MaxValue) ? long.MaxValue : value * 1024 * 1024;
                        break;

                    case Size.GB:
                        TotalFileSizeBytes = (value * 1024 * 1024 * 1024 > long.MaxValue) ? long.MaxValue : value * 1024 * 1024 * 1024;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Unexpected size unit.");
                }
            }
        }

        private void UpdateTotalChunkSize()
        {
            int value;
            if (int.TryParse(ChunkSize, out value))
            {
                switch (ChunkSizeUnits)
                {
                    case Size.B:
                        TotalChunkSizeBytes = value;
                        break;

                    case Size.KB:
                        TotalChunkSizeBytes = (value * 1024 > MaxIntArrayLengthx64) ? MaxIntArrayLengthx64 : value * 1024;
                        break;

                    case Size.MB:
                        TotalChunkSizeBytes = (value * 1024 * 1024 > MaxIntArrayLengthx64) ? MaxIntArrayLengthx64 : value * 1024 * 1024;
                        break;

                    case Size.GB:
                        TotalChunkSizeBytes = (value * 1024 * 1024 * 1024 > MaxIntArrayLengthx64) ? MaxIntArrayLengthx64 : value * 1024 * 1024 * 1024;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Unexpected size unit.");
                }
            }
        }

        private void VisitProjectHomepage()
        {
            LaunchUrlInBrowser(Settings.Default.ProjectHomepageUri);
        }

        private static void LaunchUrlInBrowser(string target)
        {
            try
            {
                Process.Start(target);
            }
            catch (Win32Exception ex)
            {
                if (ex.ErrorCode == -2147467259)
                    MessageBox.Show(string.Format("Exception encountered while launching URL '{0}':\r\n\r\nNo Internet browser was found.", target), "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                if (!ex.IsCritical())
                    MessageBox.Show(string.Format("Exception encountered while launching URL '{0}':\r\n\r\n{1}", target, ex.Message), "", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    throw;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private async void CreateAndCancelButtonPressed() // TODO: change to backgroundworker, remove async, review process when error occurs
        {
            try
            {
                if (!OperationInProgress)
                {
                    // File creation requested
                    StatusMessage = "0% file completed.";
                    ProgressPercent = 0;
                    OperationInProgress = true;

                    var j = new Models.JunkFile();
                    j.ProgressChangedEvent += (sender, e) =>
                        {
                            StatusMessage = string.Format("{0}% file completed.", e.PercentComplete);
                            ProgressPercent = e.PercentComplete;
                        };

                    cancellationTokenSource = new CancellationTokenSource();

                    var timeTaken = await j.Create(new FileInfo(FilePath), TotalFileSizeBytes, TotalChunkSizeBytes, ChunkDataType == ChunkData.Randoms ? true : false, cancellationTokenSource);

                    OperationInProgress = false;

                    if (cancellationTokenSource.IsCancellationRequested)
                        StatusMessage = "Cancelled.";
                    else
                    {
                        StatusMessage = string.Format("Completed in {0:dd\\.hh\\:mm\\:ss\\.ff} ({1}).", timeTaken, ((long)((long)TotalFileSizeBytes / timeTaken.TotalSeconds)).GenerateBandwidthString());
                    }
                }
                else
                {
                    // Cancel operation
                    if (!cancellationTokenSource.IsCancellationRequested)
                    {
                        cancellationTokenSource.Cancel();
                        StatusMessage = "Cancelling... Please wait.";
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.IsCritical())
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    throw;
            }
        }
    }
}