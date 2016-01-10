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

        public BookPageViewModel(BookService bookService,
            SectionService sectionService, IRFIDManagerDialog rfidDialogService)
        {
            _BookService = bookService;
            _SectionService = sectionService;
            _RFIDDialogService = rfidDialogService;



            items = new ObservableCollection<BookInfo>(_BookService.GetBooks());
            ItemsCollectionView = (ICollectionView)CollectionViewSource.GetDefaultView(items);

            LoadSections();
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
        }

        public override void DeleteItemCommandHandler()
        {
            throw new NotImplementedException();
        }

        public override void EditItemCommandHandler()
        {
            throw new NotImplementedException();
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
