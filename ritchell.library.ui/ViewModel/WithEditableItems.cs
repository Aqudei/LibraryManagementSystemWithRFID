using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ritchell.library.ui.ViewModel
{
    public abstract class WithEditableItems<T> : ViewModelBase where T : class
    {
        #region "UI crud management"
        private UIState uiState;
        enum UIState
        {
            Standby, Adding, Editing
        }
        #endregion "UI crud management"

        public WithEditableItems()
        {
            uiState = UIState.Standby;
        }


        public ICollectionView ItemsCollectionView { get; set; }

        protected ObservableCollection<T> items;

        private RelayCommand _SaveNewItemCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveItemCommand
        {
            get
            {
                return _SaveNewItemCommand
                    ?? (_SaveNewItemCommand = new RelayCommand(
                    () =>
                    {
                        SaveItemCommandHandler();
                        uiState = UIState.Standby;
                    },
                    () => (uiState == UIState.Adding || uiState == UIState.Editing) && InputFieldsAreValid()));
            }
        }

        public virtual bool InputFieldsAreValid()
        {
            return true;
        }

        protected abstract void NewItemCommandHandler();
        protected abstract void SaveItemCommandHandler();

        private RelayCommand _NewItemCommand;

        /// <summary>
        /// Gets the NewItemCommand.
        /// </summary>
        public RelayCommand NewItemCommand
        {
            get
            {
                return _NewItemCommand
                    ?? (_NewItemCommand = new RelayCommand(
                    () =>
                    {
                        NewItemCommandHandler();
                        uiState = UIState.Adding;
                    }, () => uiState == UIState.Standby));
            }
        }

        private RelayCommand _DeleteItemCommand;

        /// <summary>
        /// Gets the NewItemCommand.
        /// </summary>
        public RelayCommand DeleteItemCommand
        {
            get
            {
                return _DeleteItemCommand
                    ?? (_DeleteItemCommand = new RelayCommand(
                    () =>
                    {
                        DeleteItemCommandHandler();
                    }, () => uiState == UIState.Standby));
            }
        }

        public abstract void DeleteItemCommandHandler();

        private RelayCommand _EditItemCommand;

        public RelayCommand EditItemCommand
        {
            get
            {
                return _EditItemCommand = _EditItemCommand ?? new RelayCommand(
                    () =>
                    {
                        EditItemCommandHandler();
                        uiState = UIState.Editing;
                    }, () => uiState == UIState.Standby && HasSelectedItem());
            }
        }

        private bool HasSelectedItem()
        {
            return ItemsCollectionView != null && ItemsCollectionView.CurrentItem != null;
        }

        public abstract void EditItemCommandHandler();
    }
}
