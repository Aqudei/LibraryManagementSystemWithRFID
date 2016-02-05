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
    public class BookCopyPageViewModel : ViewModelBase, IDataErrorInfo
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
        private string _AcquisitionNumber;

        public string AcquisitionNumber
        {
            get { return _AcquisitionNumber; }
            set
            {
                _AcquisitionNumber = value;
                RaisePropertyChanged(() => AcquisitionNumber);
                AddBookCopyCommand.RaiseCanExecuteChanged();
            }
        }



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
                        try
                        {
                            BookCopy newBookCopy = new BookCopy();
                            newBookCopy.BookInfoId = this.CurrentBook.Id;
                            newBookCopy.BookTagShort = RFIDShort;
                            newBookCopy.BookTagLong = RFIDLong;
                            newBookCopy.AcquisitionNumber = int.Parse(AcquisitionNumber);

                            Copies.Add(newBookCopy);
                            ItemsCollectionView.MoveCurrentTo(newBookCopy);
                            BookCopyService.AddBookCopy(newBookCopy);
                            ItemsCollectionView.Refresh();
                            DialogService.ShowMessageBox("Successfully added record(s)", "Ok");
                        }
                        catch (Exception ex)
                        {
                            _Error = ex.Message;
                            DialogService.ShowMessageBox(ex.StackTrace, "Error Saving...");
                        }
                    },
                    () => CanSave));
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<IDialogService>();
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
        { }

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

        public override void Cleanup()
        {
            base.Cleanup();
            _ShortRFIDReader.StopReader();
            _LongRFIDReader.StopReader();
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

        private string _Error;
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
                string result = null;
                if (columnName == "AcquisitionNumber")
                {
                    int intAcqNu;
                    if (int.TryParse(AcquisitionNumber, out intAcqNu) == false)
                        result = "Acquisition Number must be a number with value greater than zero.";
                    else
                    {
                        if (intAcqNu <= 0)
                            result = "Acquisition Number must be a number with value greater than zero.";
                    }
                }

                CanSave = string.IsNullOrEmpty(result);
                return result;
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
