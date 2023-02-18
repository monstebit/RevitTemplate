using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Core.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using Modules.ModalWindowModule.Core;

namespace Modules.ModalWindowModule.ViewModel
{
    public class ModalWindowViewModel : INotifyPropertyChanged
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
        
        #endregion

        #region Init view model

        public ModalWindowViewModel(RevitContextHelper revitTaskHelper)
        {
            _revitTaskHelper = revitTaskHelper;
            _creator = new Creator(this);
            DeleteSelectedElementCommand = new RelayCommand(DeleteSelectedElement);
        }

        #endregion

        #region Commands

        public RelayCommand DeleteSelectedElementCommand { get; }

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
