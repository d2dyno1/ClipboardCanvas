﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace ClipboardCanvas.ViewModels.Dialogs
{
    public class DeleteConfirmationDialogViewModel : ObservableObject
    {
        #region Public Properties

        private bool _PermanentlyDelete;
        public bool PermanentlyDelete
        {
            get => _PermanentlyDelete;
            set => SetProperty(ref _PermanentlyDelete, value);
        }

        public string FileName { get; private set; }

        #endregion

        #region Constructor

        public DeleteConfirmationDialogViewModel(string fileName, bool permanentlyDelete)
        {
            this.FileName = fileName;
            this.PermanentlyDelete = permanentlyDelete;
        }

        #endregion
    }
}
