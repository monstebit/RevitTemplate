using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using DigitalServicesTools.Core.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Modules.FirstPluginModule.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class FirstPluginCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication application = commandData.Application;
            UIDocument uiDocument = application.ActiveUIDocument;
            Document document = uiDocument.Document;

            string documentName = document.Title;

            View activeView = document.ActiveView;
            List<View> viewNavisworks = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_Views)
                                                                     .Cast<View>()
                                                                     .Where(it => it.ViewType == ViewType.ThreeD && it.IsTemplate == false && it.Name != "Navisworks")
                                                                     .ToList();

            if (!(activeView is View3D))
            {
                TaskDialog errorDialog = new TaskDialog("Ошибка")
                {
                    MainInstruction = "Данная команда предназначена только для работы на 3D видах",
                    //VerificationText = "Дополнительное окно",
                    CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No,
                    DefaultButton = TaskDialogResult.Yes,
                    //FooterText = "<a href=\"https://bim.vc/edu/courses\">" + "Получить дополнительные сведения</a>"
                };

                errorDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Открыть первый попавшийся 3D вид");
                errorDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Завершить работу команды");
                TaskDialogResult dialogResult = errorDialog.Show();

                //if (errorDialog.WasVerificationChecked())
                //{
                //    TaskDialog.Show("Дополнительное окно", "Привет!)");
                //}

                if (dialogResult == TaskDialogResult.CommandLink1 || dialogResult == TaskDialogResult.Yes)
                {
                    if (viewNavisworks.Count <= 0)
                    {
                        TaskDialog.Show("Ошибка", "Создайте хотя бы один экземпляр 3D вида");
                        return Result.Cancelled;
                    }

                    uiDocument.ActiveView = viewNavisworks[0];
                }

                else if (dialogResult == TaskDialogResult.No)
                {
                    TaskDialog.Show("Предупреждение", "Команда прервана");
                    return Result.Cancelled;
                }
                else
                {
                    return Result.Failed;
                }
            }

            Transaction transaction = new Transaction(document, "Creating views");
            transaction.Start();

            FilteredElementCollector elementsOnView = new FilteredElementCollector(document, activeView.Id); // ищем все элементы на активном 3D виде

            //IList<Element> allSystems = elementsOnView.OfClass(typeof(MEPSystem)) 
            //                                           .ToElements(); // ищем все системы, которым принадлежат элементы с активного 3D вида

            OverrideGraphicSettings settingGraphics = new OverrideGraphicSettings();
            List<View> allThreeDViews = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_Views)
                                                                         .Cast<View>()
                                                                         .Where(it => it.ViewType == ViewType.ThreeD)
                                                                         .ToList(); // лист всех 3D видов в проекте

            List<string> allThreeDNames = new List<string>();
            foreach (var view in allThreeDViews)
            {
                string threeDViewName = view.Name;
                allThreeDNames.Add(threeDViewName);
            }

            string debugStringViewsNames = String.Join(", ", allThreeDNames);

            List<FilterElement> allFilters = new FilteredElementCollector(document).OfClass(typeof(ParameterFilterElement))
                                                                                              .Cast<FilterElement>()
                                                                                              .ToList(); //лист всех фильтров в проекте

            List<string> allFilterNames = new List<string>();
            foreach (var filter in allFilters)
            {
                string filterName = filter.Name;

                allFilterNames.Add(filterName);
            }

            string debugStringFilterNames = String.Join(", ", allFilterNames);

            //var mepCategories = RevitParameterHelper.GetCategories(document, BuiltInParameter.RBS_SYSTEM_NAME_PARAM, false); // все MEP категории с параметром "Имя системы"
            //var mepCategoriesIds = mepCategories.Select(category => category.Id).ToList(); // все Id MEP категорий с параметром "Имя системы" в проекте

            List<string> usedLsrSystemNames = new List<string>();
            List<string> idsFamilyInstanceReadOnly = new List<string>(); // лист id readOnly семейств
            List<Element> familyInstanceWithoutSubElements = new List<Element>(); // лист FamilyInstance без вложенных семейств

            var mepCategories = RevitParameterHelper.GetCategories(document, "LSR_Имя системы", false); // все MEP категории с параметром "LSR_Имя системы"
            var mepCategoriesIds = mepCategories.Select(category => category.Id).ToList(); // все Id MEP категорий с параметром "LSR_Имя системы" в проекте
            var elementMulticategoryFilter = new ElementMulticategoryFilter(mepCategoriesIds);
            var mepElemsOnView = new FilteredElementCollector(document, activeView.Id).WhereElementIsNotElementType().WherePasses(elementMulticategoryFilter).ToList();
            var allMepElems = new FilteredElementCollector(document).WhereElementIsNotElementType().WherePasses(elementMulticategoryFilter).ToElements();
            var listSubComp = new List<FamilyInstance>();

            foreach (var element in allMepElems)
            {
                if (element is FamilyInstance familyInstance)
                {
                    if (familyInstance.SuperComponent is not null) // вложенное семейство
                    {
                        listSubComp.Add(familyInstance);

                        continue;
                    }

                    List<ElementId> IdsSubElementReadOnly = new List<ElementId>();

                    string familyInstanceId = familyInstance.Id.ToString();
                    var subElemSet = familyInstance.GetSubComponentIds().Select(e => document.GetElement(e)).ToList(); // список вложенных семейства
                    var familyInstanceValueParamNameSystemAsString = familyInstance.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM).AsString(); // значение параметра "Имя системы" всех MEP семейств
                    var elementParamLsrNameSystem = familyInstance.LookupParameter("LSR_Имя системы"); // значение параметра "LSR_Имя системы" всех MEP семейств

                    elementParamLsrNameSystem.Set(familyInstanceValueParamNameSystemAsString); // ДАТЬ СЕМЕЙСТВУ                    
                }
                else
                {
                    // заполнение параметра системы элементам, которые не являются FamilyInstance

                    var elementValueParamNameSystemAsString = element.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM).AsString();

                    element.LookupParameter("LSR_Имя системы").Set(elementValueParamNameSystemAsString);
                }
            }

            foreach (var subElement in listSubComp)
            {
                var superComponent = subElement.SuperComponent;
                var systemParameterSuperComponent = superComponent.get_Parameter(BuiltInParameter.RBS_SYSTEM_NAME_PARAM).AsString();
                var subElementParamLsrNameSystem = subElement.LookupParameter("LSR_Имя системы");

                if (subElementParamLsrNameSystem.IsReadOnly == false)
                {
                    subElementParamLsrNameSystem.Set(systemParameterSuperComponent);
                }
                else
                {
                    continue;
                }
            }

            foreach (Element element in mepElemsOnView)
            {
                var systemName = element.LookupParameter("LSR_Имя системы").AsString();
                if (usedLsrSystemNames.Contains(systemName)) continue;
                usedLsrSystemNames.Add(systemName);
            }

            foreach (string nameSystem in usedLsrSystemNames)
            {
                if (nameSystem == null) continue;
                // виды
                var viewName = "Изометрия_Система " + nameSystem; // генерация названия 3D вида
                bool hasNameView = debugStringViewsNames.Contains(viewName); // проверяем, есть ли строка viewName в списке имен всех 3D видов в проекте

                if (hasNameView) continue;

                ElementId temp3DViewId = activeView.Duplicate(ViewDuplicateOption.WithDetailing); // дублирование активного 3D вида
                Element temp3DViewElement = document.GetElement(temp3DViewId);
                View3D temp3DView = (View3D)temp3DViewElement;

                temp3DView.Name = viewName;

                // фильтры
                var lsrSystemNameParameter = RevitParameterHelper.GetSharedParameter(document, "LSR_Имя системы");

                FilterRule rule = ParameterFilterRuleFactory.CreateNotEqualsRule(lsrSystemNameParameter.Id, nameSystem, true);

                var elementFilter = new ElementParameterFilter(rule);
                var filterName = "MACROS_Система " + nameSystem;
                bool hasNameFilter = debugStringFilterNames.Contains(filterName); // проверяем, есть ли строка filterName в списке имен всех 3D видов в проекте

                if (hasNameFilter) continue;
                ParameterFilterElement newFilter = ParameterFilterElement.Create(document, filterName, mepCategoriesIds, elementFilter);

                temp3DView.SetFilterOverrides(newFilter.Id, settingGraphics);
                temp3DView.SetFilterVisibility(newFilter.Id, false);
            }

            transaction.Commit();

            return Result.Succeeded;
        }
    }
}


