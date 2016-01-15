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

namespace ritchell.library.ui.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class BookCopyPageViewModel : ViewModelBase
    {
        private BookCopyService BookCopyService;
        private BookInfo _CurrentBook;
        private string _RFIDLong;
        private string _RFIDShort;
        private RelayCommand _AddBookCopyCommand;
        private RelayCommand _DeleteBookCopyCommand;
        private ICollectionView _ItemsCollectionView;
        private ObservableCollection<BookCopy> Copies;
        private IRFIDReader _ShortRFIDReader;
        private IRFIDReader _LongRFIDReader;

        public BookCopyPageViewModel(BookCopyService bookCopyService)
        {
            this.BookCopyService = bookCopyService;
            if (IsInDesignMode == false)
                SetupRFIDReader();
        }

        private void SetupRFIDReader()
        {
            _ShortRFIDReader = SimpleIoc.Default.GetInstance<IRFIDReader>("short");
            _LongRFIDReader = SimpleIoc.Default.GetInstance<IRFIDReader>("long");

            _ShortRFIDReader.TagRead += _ShortRFIDReader_TagRead;
            _LongRFIDReader.TagRead += _LongRFIDReader_TagRead;
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

        public ICollectionView ItemsCollectionView
        {
            get { return _ItemsCollectionView; }
            set
            {
                _ItemsCollectionView = value;
                RaisePropertyChanged(() => ItemsCollectionView);
            }
        }

        /// <summary>
        /// Gets the AddBookCopyCommand.
        /// </summary>
        public RelayCommand AddBookCopyCommand
        {
            get
            {
                return _AddBookCopyCommand
                    ?? (_AddBookCopyCommand = new RelayCommand(
                    () =>
                    {
                        BookCopy newBookCopy = new BookCopy();
                        newBookCopy.BookInfoId = this.CurrentBook.Id;
                        newBookCopy.BookTagShort = RFIDShort;
                        newBookCopy.BookTagLong = RFIDLong;

                        Copies.Add(newBookCopy);
                        ItemsCollectionView.MoveCurrentTo(newBookCopy);
                        BookCopyService.AddBookCopy(newBookCopy);
                        ItemsCollectionView.Refresh();
                    },
                    () => true));
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
                        Copies.Remove(selectedCopy);
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

                Copies = new ObservableCollection<BookCopy>(BookCopyService.BookCopiesOf(value.Id));
                ItemsCollectionView = CollectionViewSource.GetDefaultView(Copies);

                ItemsCollectionView.CurrentChanged += ItemsCollectionView_CurrentChanged;
            }
        }

        void ItemsCollectionView_CurrentChanged(object sender, EventArgs e)
        {

        }

        public string RFIDLong
        {
            set
            {
                _RFIDLong = value;
                RaisePropertyChanged(() => RFIDLong);
            }
            get
            {
                return _RFIDLong;
            }
        }

        public string RFIDShort
        {
            get
            {
                return _RFIDShort;
            }
            set
            {
                _RFIDShort = value;
                RaisePropertyChanged(() => RFIDShort);
            }
        }
    }
}
