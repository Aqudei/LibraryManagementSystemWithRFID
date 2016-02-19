using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.CommandWpf;
using ritchell.library.infrastructure.Hardware;
using ritchell.library.model;
using ritchell.library.model.Services;
using GalaSoft.MvvmLight.Ioc;

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class BookCopyPageViewModel : WithEditableItems<BookCopy>, IDataErrorInfo
    {
        private BookCopyService BookCopyService;
        private BookInfo _CurrentBook;

        private RelayCommand _DeleteBookCopyCommand;
        private IRFIDReader _ShortRFIDReader;
        private IRFIDReader _LongRFIDReader;


        public BookCopyPageViewModel(BookCopyService bookCopyService)
        {
            BookCopyService = bookCopyService;
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
            var current = ItemsCollectionView.CurrentItem as BookCopy;
            if (current != null && current.BookTagLong != e)
            {
                current.BookTagLong = e;
                RaisePropertyChanged(() => ItemsCollectionView);
            }
        }

        void _ShortRFIDReader_TagRead(object sender, string e)
        {
            var current = ItemsCollectionView.CurrentItem as BookCopy;
            if (current != null && current.BookTagShort != e)
            {
                current.BookTagShort = e;
                RaisePropertyChanged(() => ItemsCollectionView);
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
        {
            var current = ItemsCollectionView.CurrentItem as BookCopy;
            if (current != null)
            {
                current.BookTagLong = "";
                current.BookTagShort = "";
            }
        }



        private string _Error = "";
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
