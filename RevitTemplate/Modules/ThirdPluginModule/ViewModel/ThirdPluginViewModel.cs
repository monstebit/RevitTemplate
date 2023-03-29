using Autodesk.Revit.DB;
using Core.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using Modules.ThirdPluginModule.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Modules.ThirdPluginModule.ViewModel
{
    public class ThirdPluginViewModel : INotifyPropertyChanged
    {
        public RevitContextHelper Helper { get; set; }

        public RevitSelector Selector { get; set; }
        public RevitLevelChanger LevelChanger { get; set; } 

        public Element _selectedNewLevel { get; set; } 

        public Element SelectedNewLevel 
        {
            get { return _selectedNewLevel; } 
            set
            {
                _selectedNewLevel = value; 
                OnPropertyChanged(); 
            }
        }

        public Element _selectedLevel { get; set; }

        public Element SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                _selectedLevel = value;
                OnPropertyChanged();
            }
        }

        /* Пример с параметром Комментарий
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
        */

        // Выделить элементы выбранного уровня
        public ICommand MepElemsLevelChangerCommand { get; set; } 
        public void LevelChangerCore() 
        {
            Helper.RunTask(LevelChanger.LevelChangerCore); 
        } 

        public ICommand SelectLevelMepElemsCommand { get; set; }

        public void SelectLevelMepElems()
        {
            Helper.RunTask(Selector.SelectLevelMepElems);
        }

        public ObservableCollection<Element> _selectedLevels { get; set; }

        public ObservableCollection<Element> SelectedLevels
        {
            get { return _selectedLevels; }
            set
            {
                _selectedLevels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Element> _selectedNewLevels { get; set; }

        public ObservableCollection<Element> SelectedNewLevels
        {
            get { return _selectedNewLevels; }
            set
            {
                _selectedNewLevels = value;
                OnPropertyChanged();
            }
        }

        public ThirdPluginViewModel(RevitContextHelper helperOtIlya)
        {
            Helper = helperOtIlya;

            Selector = new RevitSelector(this);
            LevelChanger = new RevitLevelChanger(this);
            
            Selector.UpdateSelectedLevelsToViewModel();  
            LevelChanger.UpdateSelectedNewLevelsToViewModel(); 

            SelectLevelMepElemsCommand = new RelayCommand(SelectLevelMepElems);
            MepElemsLevelChangerCommand = new RelayCommand(LevelChangerCore); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
