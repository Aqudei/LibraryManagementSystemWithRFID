using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ritchell.library.model;
using ritchell.library.model.Services;
using System.Collections.ObjectModel;

namespace ritchell.library.ui.client.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SearchBooksViewModel : ViewModelBase
    {

        /// <summary>
        /// Initializes a new instance of the SearchBooksViewModel class.
        /// </summary>
        public SearchBooksViewModel()
        {
            var rslt = _BookSearchService.Search();
            BookSearchResult = new ObservableCollection<BookSearchResultDTO>(rslt);
        }

        private BookSearchService _BookSearchService
        {
            get
            {
                return SimpleIoc.Default.GetInstance<BookSearchService>();
            }
        }



        private ObservableCollection<BookSearchResultDTO> _BookSearchResult;

        public ObservableCollection<BookSearchResultDTO> BookSearchResult
        {
            get { return _BookSearchResult; }
            set
            {
                _BookSearchResult = value;
                RaisePropertyChanged(() => BookSearchResult);
            }
        }


        private RelayCommand _StartSearch;

        /// <summary>
        /// Gets the StartSearch    .
        /// </summary>
        public RelayCommand StartSearch
        {
            get
            {
                return _StartSearch
                    ?? (_StartSearch = new RelayCommand(
                    () =>
                    {
                        var rslt = _BookSearchService.Search(Keyword);
                        BookSearchResult = new ObservableCollection<BookSearchResultDTO>(rslt);
                    },
                    () => true));
            }
        }

        public string Keyword
        {
            get
            {
                return _Keyword;
            }

            set
            {
                _Keyword = value;
                RaisePropertyChanged(() => Keyword);
            }
        }

        private string _Keyword;
    }
}