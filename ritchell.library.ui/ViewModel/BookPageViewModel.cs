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

namespace ritchell.library.ui.ViewModel
{
    public class BookPageViewModel : WithEditableItems<BookInfo>
    {
        private BookService _BookService;
        private SectionService _SectionService;

        public BookPageViewModel(BookService bookService,
            SectionService sectionService)
        {
            _BookService = bookService;
            _SectionService = sectionService;

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
            _BookService.EnrollOrUpdateBook(ItemsCollectionView.CurrentItem as BookInfo);
        }
    }
}
