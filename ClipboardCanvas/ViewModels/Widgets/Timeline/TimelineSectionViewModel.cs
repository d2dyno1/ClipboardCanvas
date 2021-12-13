﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Threading.Tasks;

using ClipboardCanvas.Models;
using ClipboardCanvas.Extensions;
using ClipboardCanvas.Models.Configuration;
using ClipboardCanvas.Helpers;
using ClipboardCanvas.DataModels;
using ClipboardCanvas.EventArguments;
using ClipboardCanvas.Services;
using ClipboardCanvas.Enums;
using ClipboardCanvas.Helpers.SafetyHelpers;

namespace ClipboardCanvas.ViewModels.Widgets.Timeline
{
    public class TimelineSectionViewModel : ObservableObject, IDisposable
    {
        #region Properties

        private INavigationService NavigationService { get; } = Ioc.Default.GetService<INavigationService>();

        public ObservableCollection<TimelineSectionItemViewModel> Items { get; private set; }

        public DateTime SectionDate { get; private set; }

        private string _FormattedTime;
        public string FormattedTime
        {
            get => _FormattedTime;
            set => SetProperty(ref _FormattedTime, value);
        }

        private bool _IsSectionEmpty = true;
        public bool IsSectionEmpty
        {
            get => _IsSectionEmpty;
            set => SetProperty(ref _IsSectionEmpty, value);
        }

        #endregion

        #region Events

        public event EventHandler<RemoveSectionRequestedEventArgs> OnRemoveSectionRequestedEvent;

        #endregion

        #region Constructor

        public TimelineSectionViewModel(DateTime sectionDate)
        {
            this.SectionDate = sectionDate;

            this.Items = new ObservableCollection<TimelineSectionItemViewModel>();

            SetFormattedDate(this.SectionDate);
        }

        #endregion

        #region Event Handlers

        private void Item_OnRemoveSectionItemRequestedEvent(object sender, RemoveSectionItemRequestedEventArgs e)
        {
            RemoveItem(e.itemViewModel);
        }

        #endregion

        #region Helpers

        private void SetFormattedDate(DateTime dateTime)
        {
            if (dateTime == DateTime.Today)
            {
                FormattedTime = "Today";
            }
            else if ((DateTime.Now.DayOfYear - dateTime.DayOfYear) == 1)
            {
                FormattedTime = "Yesterday";
            }
            else
            {
                if (dateTime.Year < DateTime.Now.Year)
                {
                    FormattedTime = dateTime.ToString("dd MMMM yyyy");
                }
                else
                {
                    FormattedTime = dateTime.ToString("dd MMMM");
                }
            }
        }

        public async Task<SafeWrapper<TimelineSectionItemViewModel>> AddItem(ICollectionModel collectionModel, CanvasItem canvasItem, bool suppressSettingsUpdate = false)
        {
            var item = new TimelineSectionItemViewModel(collectionModel, canvasItem);
            return (item, await AddItemMode(item, true, suppressSettingsUpdate));
        }

        public async Task<SafeWrapper<TimelineSectionItemViewModel>> AddItemBack(ICollectionModel collectionModel, CanvasItem canvasItem, bool suppressSettingsUpdate = false)
        {
            var item = new TimelineSectionItemViewModel(collectionModel, canvasItem);
            return (item, await AddItemMode(item, false, suppressSettingsUpdate));
        }

        private async Task<SafeWrapperResult> AddItemMode(TimelineSectionItemViewModel timelineSectionItem, bool front, bool suppressSettingsUpdate = false)
        {
            if (front)
            {
                Items.AddFront(timelineSectionItem);
            }
            else
            {
                Items.Add(timelineSectionItem);
            }
            timelineSectionItem.OnRemoveSectionItemRequestedEvent += Item_OnRemoveSectionItemRequestedEvent;

            if (Items.Count > Constants.UI.Timeline.MAX_ITEMS_PER_SECTION)
            {
                RemoveItem(Items.Last(), suppressSettingsUpdate);
            }

            IsSectionEmpty = false;

            if (!suppressSettingsUpdate)
            {
                SettingsSerializationHelpers.UpdateUserTimelineSetting();
            }
            if (NavigationService.CurrentPage == DisplayPageType.Homepage)
            {
                return await timelineSectionItem.InitializeSectionItemContent();
            }
            else
            {
                return SafeWrapperResult.SUCCESS;
            }
        }

        public bool RemoveItem(TimelineSectionItemViewModel timelineSectionItem, bool suppressSettingsUpdate = false)
        {
            if (timelineSectionItem == null)
            {
                return false;
            }

            bool result = Items.Remove(timelineSectionItem);
            timelineSectionItem.OnRemoveSectionItemRequestedEvent -= Item_OnRemoveSectionItemRequestedEvent;

            if (Items.IsEmpty())
            {
                IsSectionEmpty = true;
                OnRemoveSectionRequestedEvent?.Invoke(this, new RemoveSectionRequestedEventArgs(this)); // This won't remove today section
            }

            if (!suppressSettingsUpdate)
            {
                SettingsSerializationHelpers.UpdateUserTimelineSetting();
            }

            return result;
        }

        public TimelineSectionItemViewModel FindTimelineSectionItem(CanvasItem canvasItem)
        {
            return Items.FirstOrDefault((item) => item.CanvasItem.AssociatedItem.Path == canvasItem?.AssociatedItem.Path);
        }

        public TimelineSectionConfigurationModel ConstructConfigurationModel()
        {
            TimelineSectionConfigurationModel configurationModel = new TimelineSectionConfigurationModel(SectionDate);

            foreach (var item in Items)
            {
                configurationModel.items.Add(item.ConstructConfigurationModel());
            }

            return configurationModel;
        }

        public void Clean()
        {
            foreach (var item in Items)
            {
                item.OnRemoveSectionItemRequestedEvent -= Item_OnRemoveSectionItemRequestedEvent;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Items?.DisposeAll();
        }

        #endregion
    }
}
