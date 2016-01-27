using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using ritchell.library.model;
using ritchell.library.model.Services;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using ritchell.library.ui.View.DialogInterface;

namespace ritchell.library.ui.ViewModel
{
    public class BookPageViewModel : WithEditableItems<BookInfo>
    {
        private BookService _BookService;
        private SectionService _SectionService;
        private RelayCommand _ManageRFIDCommand;
        private IRFIDManagerDialog _RFIDDialogService;
        private ObservableCollection<string> _Subjects;




        public BookPageViewModel(BookService bookService,
            SectionService sectionService, IRFIDManagerDialog rfidDialogService)
        {
            _BookService = bookService;
            _SectionService = sectionService;
            _RFIDDialogService = rfidDialogService;



            items = new ObservableCollection<BookInfo>(_BookService.GetBooks());
            ItemsCollectionView = (ICollectionView)CollectionViewSource.GetDefaultView(items);

            LoadSections();
            LoadSubjects();
        }

        public ObservableCollection<string> Subjects
        {
            get { return _Subjects; }
            set
            {
                _Subjects = value;
                RaisePropertyChanged(() => Subjects);
            }
        }

        private void LoadSubjects()
        {
            Subjects = new ObservableCollection<string>(_BookService.GetDistincSubjects());
        }

        private void LoadSections()
        {
            BookSectionsCollectionView = (ICollectionView)CollectionViewSource.GetDefaultView(_SectionService.GetAllSections());
            RaisePropertyChanged(() => BookSectionsCollectionView);
        }

        public ICollectionView BookSectionsCollectionView
        {
            get;
            set;
        }

        protected override void NewItemCommandHandler()
        {
            BookInfo book = new BookInfo();
            items.Add(book);
            ItemsCollectionView.MoveCurrentTo(book);
        }

        protected override void SaveItemCommandHandler()
        {
            _BookService.AddOrUpdateBook(ItemsCollectionView.CurrentItem as BookInfo);
            LoadSubjects();
        }

        public override void DeleteItemCommandHandler()
        {
            try
            {
                var currentBook = ItemsCollectionView.CurrentItem as BookInfo;
                if (currentBook != null)
                {
                    _BookService.DeleteBook(currentBook);
                    items.Remove(currentBook);
                }
            }
            catch (Exception)
            { }

        }

        public override void EditItemCommandHandler()
        {

        }

        public override bool InputFieldsAreValid()
        {
            return true;
        }

        /// <summary>
        /// Gets the ManageRFIDCommand.
        /// </summary>
        public RelayCommand ManageRFIDCommand
        {
            get
            {
                return _ManageRFIDCommand
                    ?? (_ManageRFIDCommand = new RelayCommand(
                    () =>
                    {
                        _RFIDDialogService.Manage(this.ItemsCollectionView.CurrentItem as BookInfo);
                    },
                    () => this.ItemsCollectionView.CurrentItem != null));
            }
        }
    }
}
