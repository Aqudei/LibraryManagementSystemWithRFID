using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ritchell.library.ui.ViewModel
{
    public abstract class WithEditableItems<T> : ViewModelBase where T : class
    {
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
                    },
                    () => true));
            }
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
                    }, () => true));
            }
        }
    }
}
