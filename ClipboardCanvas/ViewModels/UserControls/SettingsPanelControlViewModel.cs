﻿using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ClipboardCanvas.Enums;
using ClipboardCanvas.DataModels.Navigation;

namespace ClipboardCanvas.ViewModels.UserControls
{
    public class SettingsPanelControlViewModel : ObservableObject
    {
        #region Public Properties

        private SettingsFrameNavigationDataModel _CurrentPageNavigation;
        public SettingsFrameNavigationDataModel CurrentPageNavigation
        {
            get => _CurrentPageNavigation;
            private set => SetProperty(ref _CurrentPageNavigation, value);
        }

        #endregion

        #region Commands

        public ICommand ItemInvokedCommand { get; private set; }

        #endregion

        #region Constructor

        public SettingsPanelControlViewModel()
        {
            CurrentPageNavigation = new SettingsFrameNavigationDataModel(SettingsPageType.General);

            // Create commands
            ItemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(ItemInvoked);
        }

        #endregion

        #region Command Implementation

        private void ItemInvoked(NavigationViewItemInvokedEventArgs e)
        {
            switch (e.InvokedItemContainer.Tag?.ToString())
            {
                case "General":
                    {
                        CurrentPageNavigation = new SettingsFrameNavigationDataModel(SettingsPageType.General);

                        break;
                    }

                case "Pasting":
                    {
                        CurrentPageNavigation = new SettingsFrameNavigationDataModel(SettingsPageType.Pasting);

                        break;
                    }

                case "About":
                    {
                        CurrentPageNavigation = new SettingsFrameNavigationDataModel(SettingsPageType.About);

                        break;
                    }
            }
        }

        #endregion
    }
}
