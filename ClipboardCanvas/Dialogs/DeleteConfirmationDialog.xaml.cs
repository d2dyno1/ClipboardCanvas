﻿using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

using ClipboardCanvas.ViewModels.Dialogs;
using ClipboardCanvas.Enums;
using ClipboardCanvas.Helpers;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ClipboardCanvas.Dialogs
{
    public sealed partial class DeleteConfirmationDialog : ContentDialog, IDialog<DeleteConfirmationDialogViewModel>
    {
        public DeleteConfirmationDialogViewModel ViewModel
        {
            get => (DeleteConfirmationDialogViewModel)DataContext;
            set => DataContext = value;
        }

        public DeleteConfirmationDialog()
        {
            this.InitializeComponent();
        }

        public async new Task<DialogResult> ShowAsync() => (await base.ShowAsync()).ToDialogResult();
    }
}
