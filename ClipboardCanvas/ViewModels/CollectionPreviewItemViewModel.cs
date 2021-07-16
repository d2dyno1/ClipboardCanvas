﻿using ClipboardCanvas.Interfaces.Search;
using ClipboardCanvas.Models;
using ClipboardCanvas.Models.Implementation;
using ClipboardCanvas.ViewModels.Pages;
using ClipboardCanvas.ViewModels.UserControls;
using ClipboardCanvas.ViewModels.UserControls.Collections;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

namespace ClipboardCanvas.ViewModels
{
    public class CollectionPreviewItemViewModel : ObservableObject, ISearchItem, IDisposable
    {
        #region Public Properties

        private string _DisplayName;
        public string DisplayName
        {
            get => _DisplayName;
            private set => SetProperty(ref _DisplayName, value);
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set => SetProperty(ref _IsSelected, value);
        }

        private bool _IsHighlighted;
        public bool IsHighlighted
        {
            get => _IsHighlighted;
            set => SetProperty(ref _IsHighlighted, value);
        }

        private bool _IsCanvasPreviewVisible;
        public bool IsCanvasPreviewVisible
        {
            get => _IsCanvasPreviewVisible;
            set => SetProperty(ref _IsCanvasPreviewVisible, value);
        }

        public IReadOnlyCanvasPreviewModel SimpleCanvasPreviewModel { get; private set; }

        public ICollectionModel CollectionModel { get; private set; }

        public ICollectionItemModel CollectionItemModel { get; private set; }

        public IControlPropertyAccessorModel<IReadOnlyCanvasPreviewModel> SimpleCanvasPreviewModelAccessor { get; private set; }

        #endregion

        #region Commands

        public ICommand SimpleCanvasLoadedCommand { get; set; }

        #endregion

        #region Constructor

        public CollectionPreviewItemViewModel()
        {
            // Create commands
            SimpleCanvasLoadedCommand = new AsyncRelayCommand<RoutedEventArgs>(SimpleCanvasLoaded);
        }

        #endregion

        #region Command Implementation

        private async Task SimpleCanvasLoaded(RoutedEventArgs e)
        {
            await SimpleCanvasPreviewModel.TryLoadExistingData(CollectionItemModel, CollectionPreviewPageViewModel.LoadCancellationToken.Token);
            IsCanvasPreviewVisible = true;
        }

        #endregion

        #region Event Handlers

        private void SimpleCanvasPreviewModelAccessor_OnPropertyValueUpdatedEvent(object sender, IReadOnlyCanvasPreviewModel e)
        {
            SimpleCanvasPreviewModel = e;
        }

        #endregion

        #region Public Helpers

        public static async Task<CollectionPreviewItemViewModel> GetCollectionPreviewItemModel(ICollectionModel collectionModel, ICollectionItemModel collectionItemModel)
        {
            CollectionPreviewItemViewModel viewModel = new CollectionPreviewItemViewModel()
            {
                SimpleCanvasPreviewModelAccessor = new ControlPropertyAccessorModel<IReadOnlyCanvasPreviewModel>()
            };
            viewModel.HookEvents();

            viewModel.CollectionModel = collectionModel;
            viewModel.CollectionItemModel = collectionItemModel;

            IStorageItem sourceItem = await collectionItemModel.SourceItem;
            string itemPath;

            if (sourceItem == null)
            {
                itemPath = collectionItemModel.Item.Path;
            }
            else
            {
                itemPath = sourceItem.Path;
            }

            viewModel.DisplayName = Path.GetFileName(itemPath);

            return viewModel;
        }

        #endregion

        #region Event Hooks

        private void HookEvents()
        {
            if (SimpleCanvasPreviewModelAccessor != null)
            {
                SimpleCanvasPreviewModelAccessor.OnPropertyValueUpdatedEvent += SimpleCanvasPreviewModelAccessor_OnPropertyValueUpdatedEvent;
            }
        }

        private void UnhookEvents()
        {
            if (SimpleCanvasPreviewModelAccessor != null)
            {
                SimpleCanvasPreviewModelAccessor.OnPropertyValueUpdatedEvent -= SimpleCanvasPreviewModelAccessor_OnPropertyValueUpdatedEvent;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            SimpleCanvasPreviewModel?.Dispose();
            UnhookEvents();
        }

        #endregion
    }
}