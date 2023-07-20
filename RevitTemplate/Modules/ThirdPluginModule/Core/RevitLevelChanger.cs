using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Core.Enums;
using Core.Results;
using DigitalServicesTools.Core.Helpers;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Modules.ThirdPluginModule.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Modules.ThirdPluginModule.Core
{
    public class RevitLevelChanger
    {
        /* Эта строка кода объявляет свойство с именем ViewModel типа ThirdPluginViewModel. 
        Свойство имеет геттер и сеттер, которые позволяют получать и устанавливать значение свойства. */
        public ThirdPluginViewModel ViewModel { get; set; }

        /* Этот фрагмент кода объявляет конструктор для класса RevitLevelChanger. */
        public RevitLevelChanger(ThirdPluginViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void UpdateSelectedNewLevelsToViewModel()
        {
            var module = ThirdPluginModule.GetInstance();

            var document = module.Document;
            var uiDocument = module.UiDocument;

            // Все уровни в проекте
            FilteredElementCollector levelsCollector = new FilteredElementCollector(document);
                                     levelsCollector.OfClass(typeof(Level));

            // Сортируем список уровней
            var sortedLevelsCollector = levelsCollector.OrderBy(element => element.Name);

            // Передаём сортированные уровни в интерфейс
            ViewModel.SelectedNewLevels = new ObservableCollection<Element>(sortedLevelsCollector);
        }

        public RevitContextResult LevelChangerCore()
        {
            var selectedNewLevel = ViewModel.SelectedNewLevel;

            var module = ThirdPluginModule.GetInstance();

            var document = module.Document;
            var uiDoc = module.UiDocument;

            // Get all levels in the project
            FilteredElementCollector levelsCollector = new FilteredElementCollector(document).OfClass(typeof(Level));

            // Sort the levels by name
            var sortedLevelsCollector = levelsCollector.OrderBy(element => element.Name).ToList();

            // Set the ViewModel.SelectedNewLevels to the sorted levels list
            ViewModel.SelectedNewLevels = new ObservableCollection<Element>(sortedLevelsCollector); 

            /* Создаёт экземпляр класса RevitLevelChanger
            и передаёт ему экземпляр класса ThirdPluginViewModel в качестве параметра. */
            RevitLevelChanger LevelChanger = new RevitLevelChanger(ViewModel);

            /* revitSelector из RevitSelector
            //RevitSelector revitSelector = new RevitSelector(ViewModel); */

            /* elementIds из RevitSelector
            List<ElementId> selectedMepElemsIds = selector.elementIds; */

            Selection selectedElements = uiDoc.Selection; 
            IList<ElementId> selectedIds = (IList<ElementId>)selectedElements.GetElementIds();

            ICollection<BuiltInCategory> noOffsetCat = new List<BuiltInCategory>
            {
                // ОВ 
                BuiltInCategory.OST_DuctTerminal, 
                BuiltInCategory.OST_MechanicalEquipment,
                // ВК
                BuiltInCategory.OST_PlumbingFixtures, 
                BuiltInCategory.OST_Sprinklers, 
                // ЭОМ
                BuiltInCategory.OST_LightingDevices, 
                BuiltInCategory.OST_DataDevices, 
                BuiltInCategory.OST_LightingFixtures, 
                BuiltInCategory.OST_ElectricalFixtures, 
                BuiltInCategory.OST_ElectricalEquipment,
                BuiltInCategory.OST_SecurityDevices, 
            };

            ICollection<BuiltInCategory> OffsetCat = new List<BuiltInCategory>
            {
                // ОВ
                BuiltInCategory.OST_DuctCurves, 
                BuiltInCategory.OST_PipeCurves, 
                BuiltInCategory.OST_PlaceHolderDucts, 

                BuiltInCategory.OST_DuctAccessory,
                BuiltInCategory.OST_PipeAccessory, 
                BuiltInCategory.OST_DuctFitting, 
                BuiltInCategory.OST_PipeFitting, 
                // ВК
                BuiltInCategory.OST_FlexPipeCurves, 
                // ЭОМ
                BuiltInCategory.OST_CableTray, 
                BuiltInCategory.OST_Conduit, 
                BuiltInCategory.OST_CableTrayFitting, 
                BuiltInCategory.OST_ConduitFitting, 
            };

            List<String> errorElems = new List<String>();

            var newLevel = (Level)selectedNewLevel;

            if (selectedIds.Count == 0) return new RevitContextResult(ResultStatus.Error,
            $"Вы должны выделить элементы.");

            if (selectedNewLevel == null) return new RevitContextResult(ResultStatus.Error,
            $"Вы должны выбрать уровень для переопределения.");

            using (var tr = new Transaction(document))
            {
                tr.Start("Revit Level Changer process.");

                foreach (var elementId in selectedIds)
                {
                    Element elem = document.GetElement(elementId);

                    Element noCollectionElem = document.GetElement(elementId);

                    BuiltInCategory elemCategory = (BuiltInCategory)elem.Category.Id.IntegerValue;

                    Parameter levelParameter = RevitElementLevelParameterGetter.GetLevelParameterOfElement(elem);

                    if (levelParameter == null)
                    {
                        var noCollectionElemId = noCollectionElem.Id.IntegerValue.ToString();
                        errorElems.Add(noCollectionElemId);
                        continue;
                    }

                    if (levelParameter.IsReadOnly)
                    {
                        var noCollectionElemId = noCollectionElem.Id.IntegerValue.ToString();
                        errorElems.Add(noCollectionElemId);
                        continue;
                    }
                    else
                    {
                        // noOffsetCat
                        if (noOffsetCat.Contains(elemCategory))
                        {
                            ElementId currentLevelId = levelParameter.AsElementId();
                            Element currentLevel = document.GetElement(currentLevelId);

                            if (currentLevel != null && currentLevel.Name != selectedNewLevel.Name)
                            {
                                var elementLevel = (Level)currentLevel;
                                var elemLocationPoint = elem.Location as LocationPoint;

                                double newOffset = -(newLevel.Elevation - elementLevel.Elevation);

                                XYZ elementLocationPointZ = elemLocationPoint.Point;
                                if (elem.Pinned == true)
                                {
                                    elem.Pinned = false;
                                    levelParameter.Set(newLevel.Id);
                                    elem.Location.Move(new XYZ(0, 0, newOffset));
                                    elem.Pinned = true;
                                }
                                else if (elem.Pinned == false)
                                {
                                    levelParameter.Set(newLevel.Id);
                                    elem.Location.Move(new XYZ(0, 0, newOffset));
                                }
                            }
                        }
                        // OffsetCat
                        else if (OffsetCat.Contains(elemCategory))
                        {
                            levelParameter.Set(newLevel.Id);
                            //TaskDialog.Show("Revit", $"Элемент не категории ВРУ и Оборудование");
                        }
                        // errorElems
                        else
                        {
                            var noCollectionElemId = noCollectionElem.Id.IntegerValue.ToString();
                            errorElems.Add(noCollectionElemId);
                        }
                    }
                }

                tr.Commit();
            }

            if (errorElems.Count == 0)
            {
                //TaskDialog.Show("Выполнено", "Команда выполнена.");
                return new RevitContextResult(ResultStatus.Complete,
                $"Команда выполнена успешно.");
            }
            else
            {
                string errorElemsNameString = string.Join(", ", errorElems);
                //MessageBox.Show($"Команда выполнена, но не для вех элементов.\nIds: {errorElemsNameString}");

                return new RevitContextResult(ResultStatus.Complete,
                $"Команда выполнена, но не для вех элементов.\nIds: {errorElemsNameString}");
            }
        }
    }
}
