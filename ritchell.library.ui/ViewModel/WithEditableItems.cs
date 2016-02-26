using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.infrastructure.Logging;
using ritchell.library.model.Logging;
using ritchell.library.model;

namespace ritchell.library.ui.ViewModel
{
    public abstract class WithEditableItems<T> : ViewModelBase where T : class
    {
        #region "UI crud management"

        private UIState userInterfaceState;

        public UIState UserInterfaceState
        {
            get
            {
                return userInterfaceState;
            }

            set
            {
                userInterfaceState = value;
                RaisePropertyChanged(() => UserInterfaceState);
                RaisePropertyChanged(() => EditingEnabled);
            }
        }

        public bool EditingEnabled
        {
            get
            {
                return UserInterfaceState != UIState.Standby;
            }
        }


        public enum UIState
        {
            Standby, Adding, Editing
        }

        #endregion "UI crud management"

        public WithEditableItems()
        {
            UserInterfaceState = UIState.Standby;
        }

        private ICollectionView _ItemsCollectionView;
        public ICollectionView ItemsCollectionView
        {
            get { return _ItemsCollectionView; }

            set
            {
                _ItemsCollectionView = value;
                RaisePropertyChanged(() => ItemsCollectionView);
            }
        }

        protected ObservableCollection<T> items;

        private RelayCommand _SaveNewItemCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>D:\mine_files\_dev\ritchell.library\ritchell.library.ui\ViewModel\WithEditableItems.cs
        public RelayCommand SaveItemCommand
        {
            get
            {
                return _SaveNewItemCommand
                    ?? (_SaveNewItemCommand = new RelayCommand(
                    () =>
                    {
                        try
                        {
                            SaveItemCommandHandler();

                            if (UserInterfaceState == UIState.Adding)
                            {
                                ActionLogger.Log(string.Format("{0} added new <{1}> : <{2}>.", SimpleIoc.Default.GetInstance<LibraryUser>("current_user").Username, typeof(T).Name, ItemsCollectionView.CurrentItem));
                            }
                            else if (UserInterfaceState == UIState.Editing)
                            {
                                ActionLogger.Log(string.Format("{0} edited <{1}> : <{2}>.", SimpleIoc.Default.GetInstance<LibraryUser>("current_user").Username, typeof(T).Name, ItemsCollectionView.CurrentItem));
                            }


                            UserInterfaceState = UIState.Standby;
                            DialogService.ShowMessageBox("Records successfully updated", "OK");
                        }
                        catch (Exception ex)
                        {
                            DialogService.ShowMessageBox(ex.Message, "Error");
                        }

                    },
                    () => (UserInterfaceState == UIState.Adding || UserInterfaceState == UIState.Editing) && InputFieldsAreValid()));
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<IDialogService>();
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
                        UserInterfaceState = UIState.Adding;
                    }, () => UserInterfaceState == UIState.Standby));
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
                        var toDelete = ItemsCollectionView.CurrentItem;
                        DeleteItemCommandHandler();
                        ActionLogger.Log(string.Format("{0} deleted <{1}> : <{2}>.", SimpleIoc.Default.GetInstance<LibraryUser>("current_user").Username, typeof(T).Name, toDelete));
                    }, () => UserInterfaceState == UIState.Standby));
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
                        UserInterfaceState = UIState.Editing;
                    }, () => UserInterfaceState == UIState.Standby && HasSelectedItem());
            }
        }

        private RelayCommand _CancelEditItemCommand;
        /// <summary>
        /// Gets the CancelEditItemCommand.
        /// </summary>
        public RelayCommand CancelEditItemCommand
        {
            get
            {
                return _CancelEditItemCommand
                    ?? (_CancelEditItemCommand = new RelayCommand(
                    () =>
                    {
                        if (UserInterfaceState == UIState.Adding)
                            items.Remove(ItemsCollectionView.CurrentItem as T);
                        else
                            RestoreOldData();
                        UserInterfaceState = UIState.Standby;
                    },
                    () => UserInterfaceState == UIState.Editing
                    || userInterfaceState == UIState.Adding));
            }
        }

        public virtual void RestoreOldData()
        {
            return;
        }

        private bool HasSelectedItem()
        {
            return ItemsCollectionView != null && ItemsCollectionView.CurrentItem != null;
        }

        public abstract void EditItemCommandHandler();

        private string _FilterString;

        public string FilterString
        {
            get { return _FilterString; }
            set
            {
                _FilterString = value;
                RaisePropertyChanged(() => FilterString);
            }
        }



        private RelayCommand<string> _FilterCommand;

        /// <summary>
        /// Gets the FilterCommand.
        /// </summary>
        public RelayCommand<string> FilterCommand
        {
            get
            {
                return _FilterCommand
                    ?? (_FilterCommand = new RelayCommand<string>(
                    (filterText) =>
                    {
                        if (string.IsNullOrEmpty(filterText))
                        {
                            ItemsCollectionView.Filter = new Predicate<object>((x) =>
                            {
                                return true;
                            });
                        }
                        else
                        {
                            ItemsCollectionView.Filter = GetFilter(filterText.ToUpper());
                        }

                        ItemsCollectionView.Refresh();
                    }));

            }
        }

        public IActionLogger ActionLogger
        {
            get
            {
                return _ActionLogger = _ActionLogger ?? new DBLogger();
            }
        }

        public virtual Predicate<object> GetFilter(string filterText)
        {
            return (_filterText) => true;
        }

        private IActionLogger _ActionLogger;
    }
}
