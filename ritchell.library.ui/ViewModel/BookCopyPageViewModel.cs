using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.infrastructure.Hardware;
using ritchell.library.model;
using ritchell.library.model.Services;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class BookCopyPageViewModel : WithEditableItems<BookCopy>, IDataErrorInfo
    {
        private BookCopyService BookCopyService;
        private BookInfo _CurrentBook;
        private string _RFIDLong;
        private string _RFIDShort;

        private RelayCommand _DeleteBookCopyCommand;
        private IRFIDReader _ShortRFIDReader;
        private IRFIDReader _LongRFIDReader;


        public BookCopyPageViewModel(BookCopyService bookCopyService)
        {
            this.BookCopyService = bookCopyService;
            SetupRFIDReader();
        }

        private void SetupRFIDReader()
        {
            _ShortRFIDReader = SimpleIoc.Default.GetInstance<IRFIDReader>("short");
            _LongRFIDReader = SimpleIoc.Default.GetInstance<IRFIDReader>("long");

            _ShortRFIDReader.TagRead += _ShortRFIDReader_TagRead;
            _LongRFIDReader.TagRead += _LongRFIDReader_TagRead;


            _ShortRFIDReader.StartReader();
            _LongRFIDReader.StartReader();
        }

        private void _LongRFIDReader_TagRead(object sender, string e)
        {
            if (RFIDLong != e)
            {
                RFIDLong = e;
            }
        }

        void _ShortRFIDReader_TagRead(object sender, string e)
        {
            if (RFIDShort != e)
            {
                RFIDShort = e;
            }
        }



        /// <summary>
        /// Gets the DeleteBookCopyCommand.
        /// </summary>
        public RelayCommand DeleteBookCopyCommand
        {
            get
            {
                return _DeleteBookCopyCommand
                    ?? (_DeleteBookCopyCommand = new RelayCommand(
                    () =>
                    {
                        var selectedCopy = (ItemsCollectionView.CurrentItem as BookCopy);
                        BookCopyService.RemoveBookCopy(selectedCopy.Id);
                        items.Remove(selectedCopy);
                    },
                    () => ItemsCollectionView != null && ItemsCollectionView.CurrentItem != null));
            }
        }

        public BookInfo CurrentBook
        {
            get { return _CurrentBook; }
            set
            {
                _CurrentBook = value;
                RaisePropertyChanged(() => CurrentBook);

                items = new ObservableCollection<BookCopy>(BookCopyService.BookCopiesOf(value.Id));
                ItemsCollectionView = CollectionViewSource.GetDefaultView(items);

                ItemsCollectionView.CurrentChanged += ItemsCollectionView_CurrentChanged;
            }
        }

        void ItemsCollectionView_CurrentChanged(object sender, EventArgs e)
        { }

        public string RFIDLong
        {
            set
            {
                var current = ItemsCollectionView.CurrentItem as BookCopy;
                if (current != null)
                    current.BookTagLong = value;

                _RFIDLong = value;
                RaisePropertyChanged(() => RFIDLong);
            }
            get
            {
                return _RFIDLong;
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            _ShortRFIDReader.StopReader();
            _LongRFIDReader.StopReader();
        }

        protected override void NewItemCommandHandler()
        {
            var newCopy = new BookCopy();
            items.Add(newCopy);
            ItemsCollectionView.MoveCurrentTo(newCopy);
        }

        protected override void SaveItemCommandHandler()
        {
            BookCopyService.AddOrUpdateBookCopy(ItemsCollectionView.CurrentItem as BookCopy);
            ItemsCollectionView.Refresh();
        }

        public override void DeleteItemCommandHandler()
        {
            try
            {
                var current = ItemsCollectionView.CurrentItem as BookCopy;
                BookCopyService.RemoveBookCopy(current.Id);
                items.Remove(current);
            }
            catch (Exception)
            {
                DialogService.ShowMessageBox("", "Failed To Delete");
            }
        }

        public override void EditItemCommandHandler()
        { }

        public string RFIDShort
        {
            get
            {
                return _RFIDShort;
            }
            set
            {
                var current = ItemsCollectionView.CurrentItem as BookCopy;
                if (current != null)
                    current.BookTagShort = value;
                _RFIDShort = value;
                RaisePropertyChanged(() => RFIDShort);
            }
        }

        private string _Error="";
        public string Error
        {
            get
            {
                return _Error;
            }
        }

        public string this[string columnName]
        {
            get
            {
                return string.Empty;
            }
        }

        private bool canSave;
        public bool CanSave
        {
            get { return canSave; }
            set
            {
                canSave = value;
                RaisePropertyChanged(() => CanSave);
            }
        }
    }
}
