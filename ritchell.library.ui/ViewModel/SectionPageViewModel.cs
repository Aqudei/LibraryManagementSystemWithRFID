using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ritchell.library.model;
using ritchell.library.model.Services;

namespace ritchell.library.ui.ViewModel
{
    public class SectionPageViewModel : WithEditableItems<Section>
    {
        private SectionService _SectionService;

        public SectionPageViewModel(SectionService sectionService)
        {
            _SectionService = sectionService;

            items = new ObservableCollection<Section>(_SectionService.GetAllSections());
            ItemsCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(items);
        }

        protected override void NewItemCommandHandler()
        {
            var newSection = new Section();
            items.Add(newSection);
            ItemsCollectionView.MoveCurrentTo(newSection);
        }

        protected override void SaveItemCommandHandler()
        {
            _SectionService.SaveSection(ItemsCollectionView.CurrentItem as Section);
        }
    }
}
