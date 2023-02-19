using Autodesk.Revit.DB;
using Core.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using Modules.ZeroPluginModule.Core;
using MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Modules.ZeroPluginModule.ViewModel
{
    public class ZeroPluginViewModel : INotifyPropertyChanged
    {
        public RevitSelector Selector { get; set; }
        public RevitContextHelper Helper { get; set; }
        public Element _selectedElement { get; set; }
        public Element SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                _selectedElement = value;
                OnPropertyChanged();
            }
        }

        public string _commentString { get; set; }
        public string CommentString
        {
            get { return _commentString; }
            set
            {
                _commentString = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeCommentParamValueCommand { get; set; }
        public void ChangeCommentParamValue()
        {
            Helper.RunTask(Selector.ChangeCommentParameterValue);
        }

        public ObservableCollection<Element> _selectedElements { get; set; }
        public ObservableCollection<Element> SelectedElements
        {
            get { return _selectedElements; }
            set
            {
                _selectedElements = value;
                OnPropertyChanged();
            }
        }

        public ZeroPluginViewModel(RevitContextHelper helperOtIlya)
        {
            Helper = helperOtIlya;
            Selector = new RevitSelector(this);
            Selector.UpdateSelectedElementsToViewModel();

            ChangeCommentParamValueCommand = new RelayCommand(ChangeCommentParamValue);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
