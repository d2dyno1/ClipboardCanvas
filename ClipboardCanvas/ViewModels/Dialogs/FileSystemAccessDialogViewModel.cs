﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClipboardCanvas.ViewModels.Dialogs
{
    public class FileSystemAccessDialogViewModel : ObservableObject
    {
        #region Commands

        public ICommand OpenSettingsCommand { get; private set; }

        #endregion

        #region Constructor

        public FileSystemAccessDialogViewModel()
        {
            // Create commands
            OpenSettingsCommand = new AsyncRelayCommand(OpenSettings);
        }

        #endregion

        #region Command Implementation

        private async Task OpenSettings()
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-broadfilesystemaccess"));
        }

        #endregion
    }
}
