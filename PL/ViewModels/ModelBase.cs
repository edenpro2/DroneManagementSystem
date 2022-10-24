using DalFacade.DO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PL.ViewModels
{
    public abstract class ModelBase<T> : ViewModelBase
    {
        private ObservableCollection<T> _collection;
        public ObservableCollection<T> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<T> _filtered;
        public ObservableCollection<T> Filtered
        {
            get => _filtered;
            set
            {
                _filtered = value;
                OnPropertyChanged();
            }
        }

        protected ModelBase(IEnumerable<T> enumerable)
        {
            _filtered = _collection = new ObservableCollection<T>(enumerable);
        }

        //public void Update(T item) => _collection.Insert(item.Id, item);
    }
}
