using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
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
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Modules.ThirdPluginModule.Core
{
    public class RevitSelector
    {
        /* elementIds из RevitSelector 
        public List<ElementId> elementIds { get; set; } // Для RevitLevelChanger
        */
        public ThirdPluginViewModel ViewModel { get; set; }

        public RevitSelector(ThirdPluginViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void UpdateSelectedLevelsToViewModel()
        {
            var module = ThirdPluginModule.GetInstance();

            var document = module.Document;
            var uiDocument = module.UiDocument;

            // Создаем коллектор для документа
            FilteredElementCollector levelsCollector = new FilteredElementCollector(document);

            // Применяем фильтр по классу Level
            levelsCollector.OfClass(typeof(Level));

            // Сортируем коллекцию по имени
            var sortedLevelsCollector = levelsCollector.OrderBy(element => element.Name);

            ViewModel.SelectedLevels = new ObservableCollection<Element>(sortedLevelsCollector);
        }

        public RevitContextResult SelectLevelMepElems()
        {
            var selectedLevel = ViewModel.SelectedLevel;

            var module = ThirdPluginModule.GetInstance();

            var document = module.Document;        
            var uiDoc = module.UiDocument;

            // Create a list to hold the ElementIds of the ducts and pipes that are on the selected level
            List<ElementId> elementMepIds = new List<ElementId>();

            /*Выделить все элементы с параметром уровня в зависимости от категории
            List<ElementId> allElementIds = new List<ElementId>();
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.WhereElementIsNotElementType();
            ICollection<Element> allElements = collector.ToElements();
            foreach (Element elem in allElements)
            {
                Parameter levelParameter = RevitElementLevelParameterGetter.GetLevelParameterOfElement(elem);
                if (levelParameter != null)
                {
                    allElementIds.Add(elem.Id);
                }
            }*/

            // Get the MEP CURVES categories
            ICollection<BuiltInCategory> categoriesCurves = new List<BuiltInCategory> 
            {
                BuiltInCategory.OST_DuctCurves,
                BuiltInCategory.OST_PipeCurves, 
                BuiltInCategory.OST_PlaceHolderDucts, 
                BuiltInCategory.OST_CableTray, 
                BuiltInCategory.OST_Conduit,          
            };

            // Create an Element Multicategory Filter for the categories
            ElementMulticategoryFilter filterCurves = new ElementMulticategoryFilter(categoriesCurves);

            // Create a filtered element collector to get the ducts and pipes
            FilteredElementCollector collectorCurves = new FilteredElementCollector(document);
                                     collectorCurves.WherePasses(filterCurves)
                                                    .WhereElementIsNotElementType();

            // [ Воздуховоды и трубы ]
            if (selectedLevel != null)
            {
                foreach (Element elementCurve in collectorCurves)
                {
                    // Check if the element has a Reference Level parameter
                    if (elementCurve.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM) != null)
                    {
                        // Get the element's Reference Level parameter value
                        Parameter referenceLevel = elementCurve.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM);
                        string referenceLevelName = referenceLevel.AsValueString();

                        // Check if the element's Reference Level parameter value matches the selected level name
                        if (referenceLevelName.Equals(selectedLevel.Name))
                        {
                            // Add the element's ElementId to the list
                            elementMepIds.Add(elementCurve.Id);
                        }
                    }
                }
            }
            else
            {
                return new RevitContextResult(ResultStatus.Error,
                $"Выберете уровень, на котором необходимо выделить элементы.");
            }

            // Get the  categories
            ICollection<BuiltInCategory> categoriesMep = new List<BuiltInCategory> 
            {
                // ОВ NO OFFSET categories
                BuiltInCategory.OST_DuctTerminal, // Элемент переносится с уровнем
                BuiltInCategory.OST_MechanicalEquipment, // Элемент переносится с уровнем

                // ОВ OFFSET categories
                BuiltInCategory.OST_DuctAccessory, // Элемент НЕ переносится с уровнем
                BuiltInCategory.OST_PipeAccessory, // Элемент НЕ переносится с уровнем
                BuiltInCategory.OST_DuctFitting, // Элемент НЕ переносится с уровнем
                BuiltInCategory.OST_PipeFitting, // Элемент НЕ переносится с уровнем

                // ВК NO OFFSET categories
                BuiltInCategory.OST_PlumbingFixtures, // Элемент переносится с уровнем
                BuiltInCategory.OST_Sprinklers, // Элемент переносится с уровнем

                // ВК OFFSET categories
                BuiltInCategory.OST_FlexPipeCurves, // Элемент НЕ переносится с уровнем

                // ЭОМ NO OFFSET categories
                BuiltInCategory.OST_LightingDevices, // Элемент переносится с уровнем
                BuiltInCategory.OST_DataDevices, // Элемент переносится с уровнем
                BuiltInCategory.OST_LightingFixtures, // Элемент переносится с уровнем
                BuiltInCategory.OST_ElectricalFixtures, // Элемент переносится с уровнем
                BuiltInCategory.OST_ElectricalEquipment, // Элемент переносится с уровнем
                BuiltInCategory.OST_SecurityDevices, // Элемент переносится с уровнем
                
                // ЭОМ OFFSET categories
                BuiltInCategory.OST_CableTrayFitting, // Элемент НЕ переносится с уровнем
                BuiltInCategory.OST_ConduitFitting, // Элемент НЕ переносится с уровнем
            };

            // Create an Element Multicategory Filter for the categories
            ElementMulticategoryFilter filterMep = new ElementMulticategoryFilter(categoriesMep);

            // Create a filtered element collector to get the ducts and pipes
            FilteredElementCollector collectorMep = new FilteredElementCollector(document);
                                     collectorMep.WherePasses(filterMep)
                                                 .WhereElementIsNotElementType();

            // [ Семейства MEP ]

            foreach (Element elementMep in collectorMep)
            {
                // Check if the element has a Reference Level parameter
                if (elementMep.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM) != null)
                {
                    // Get the element's Reference Level parameter value
                    Parameter referenceLevel = elementMep.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM);
                    string referenceLevelName = referenceLevel.AsValueString();

                    // Check if the element's Reference Level parameter value matches the selected level name
                    if (referenceLevelName.Equals(selectedLevel.Name))
                    {
                        // Add the element's ElementId to the list
                        elementMepIds.Add(elementMep.Id);
                    }
                }
            }   

            using (var tr = new Transaction(document))
            {
                tr.Start("RevitSelector.");

                uiDoc.Selection.SetElementIds(elementMepIds);

                tr.Commit();
            }

            /* Пример MessageBox
            List<String> errorElemsName = new List<String>();
            foreach (ElementId elem in elementMepIds)
            {
                Element noCollectionElem = document.GetElement(elem);
                var noCollectionElemId = noCollectionElem.Id.IntegerValue.ToString();
                errorElemsName.Add(noCollectionElemId);
            }
            string errorElemsNameString = string.Join(", ", errorElemsName);
            MessageBox.Show($"Элементы Id {errorElemsNameString}");
            */

            return new RevitContextResult(ResultStatus.Complete,
            $"Выделены элементы MEP, привязанные к уровню < {selectedLevel.Name} >");
        }
    }
}
