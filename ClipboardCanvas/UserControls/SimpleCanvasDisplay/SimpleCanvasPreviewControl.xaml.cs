﻿using Microsoft.UI.Xaml;

using ClipboardCanvas.Models;
using ClipboardCanvas.ModelViews;
using ClipboardCanvas.ViewModels.UserControls.CanvasPreview;
using ClipboardCanvas.Helpers;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ClipboardCanvas.UserControls.SimpleCanvasDisplay
{
    public sealed partial class SimpleCanvasPreviewControl : ObservableUserControl, IBaseCanvasPreviewControlView
    {
        public SimpleCanvasPreviewControlViewModel ViewModel
        {
            get => (SimpleCanvasPreviewControlViewModel)DataContext;
            set
            {
                DataContext = value;
                TwoWayReadOnlyCanvasPreview?.NotifyPropertyValueUpdated(value);
            }
        }

        public ICollectionModel CollectionModel { get; set; }

        private TwoWayPropertyUpdater<IReadOnlyCanvasPreviewModel> _TwoWayReadOnlyCanvasPreview;
        public TwoWayPropertyUpdater<IReadOnlyCanvasPreviewModel> TwoWayReadOnlyCanvasPreview 
        {
            get => _TwoWayReadOnlyCanvasPreview;
            set
            {
                if (_TwoWayReadOnlyCanvasPreview != value)
                {
                    _TwoWayReadOnlyCanvasPreview = value;

                    if (ViewModel != null)
                    {
                        _TwoWayReadOnlyCanvasPreview.NotifyPropertyValueUpdated(ViewModel);
                    }
                }
            }
        }


        public static readonly DependencyProperty SimpleCanvasPreviewModelProperty =
           DependencyProperty.Register(nameof(SimpleCanvasPreviewModel), typeof(IReadOnlyCanvasPreviewModel), typeof(SimpleCanvasPreviewControl), new PropertyMetadata(null));
        public IReadOnlyCanvasPreviewModel SimpleCanvasPreviewModel
        {
            get { return (IReadOnlyCanvasPreviewModel)GetValue(SimpleCanvasPreviewModelProperty); }
            set { SetValueDP(SimpleCanvasPreviewModelProperty, value); }
        }

       
        public SimpleCanvasPreviewControl()
        {
            this.InitializeComponent();

            this.ViewModel = new SimpleCanvasPreviewControlViewModel(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                SimpleCanvasPreviewModel = ViewModel;
            }
        }
    }
}
