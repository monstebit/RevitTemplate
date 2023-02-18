using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Core.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using Modules.SecondPluginModule.Core;

namespace Modules.SecondPluginModule.ViewModel
{
    public class SecondPluginViewModel : INotifyPropertyChanged
    {
        #region Properties

        private RevitContextHelper _revitTaskHelper;
        private Creator _creator;
        private Element _selectedElement;
        
        public Element SelectedElement 
        { 
            get => _selectedElement;
            set
            {
                _selectedElement = value;
                OnPropertyChanged();
            } 
        }

        //private ObservableCollection<string> _comboBoxValues = new ObservableCollection<string>();
        //public ObservableCollection<string> ComboBoxValues
        //{
        //    get { return _comboBoxValues; }
        //    set
        //    {
        //        _comboBoxValues = value;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion

        #region Init view model

        public SecondPluginViewModel(RevitContextHelper revitTaskHelper)
        {
            _revitTaskHelper = revitTaskHelper;
            _creator = new Creator(this);
            DeleteSelectedElementCommand = new RelayCommand(DeleteSelectedElement);
        }

        #endregion

        #region Commands

        public RelayCommand DeleteSelectedElementCommand { get; }

        //public ObservableCollection<ComboBoxItem> ComboBoxItems { get; internal set; }

        private void DeleteSelectedElement()
        {
            _revitTaskHelper.RunTask(_creator.Delete);
        }

        #endregion
        
        #region PropertyChanged 
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
