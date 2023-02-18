using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using DigitalServicesTools.Core.Helpers.Core;

namespace DigitalServicesTools.Core.Helpers
{
    public static class RevitParameterHelper
    {
        private static List<ParameterMetadata> GenerateParameterMetadata(Document document, Category ductCategory)
        {
            var parameterMetadata = new List<ParameterMetadata>();
            var newTable = ViewSchedule.CreateSchedule(document, ductCategory.Id);
            var availableParametersIds = ViewSchedule.GetAvailableParameters(document, ductCategory.Id);

            var allFields = newTable.Definition
                .GetSchedulableFields().Where(p => availableParametersIds.Contains(p.ParameterId));
            
            foreach (var field in allFields)
            {
                newTable.Definition.AddField(field);
            }

            var fieldCount = newTable.Definition.GetFieldCount();

            for (int i = 0; i < fieldCount; i++)
            {
                var informField = newTable.Definition.GetField(i);
                if (informField.FieldType is not ScheduleFieldType.ElementType && informField.FieldType is not ScheduleFieldType.Instance) continue;

                if (informField.GetName() == "ADSK_Завод-изготовитель")
                {
                    //var dfsdfsd = true;
                    //28648
                }
                parameterMetadata.Add(new ParameterMetadata(ductCategory, informField.FieldType, informField.ParameterId, informField.GetName()));
            }

            return parameterMetadata;
        }

        public static Dictionary<Category, List<ParameterMetadata>> GetCategoryParameterMap(Document document, List<Category> categories)
        {
            var parameterElements = new FilteredElementCollector(document)
                .OfClass(typeof(ParameterElement)).Cast<ParameterElement>().ToList();
            var systemParameterIds = Enum.GetValues(typeof(BuiltInParameter));

            var categoryParameterMetadata = new Dictionary<Category, List<ParameterMetadata>>();

            using (var transaction = new Transaction(document, "GetCategoryParameterMap"))
            {
                transaction.Start();

                foreach (var category in categories)
                {
                    var metadata = GenerateParameterMetadata(document, category);
                    categoryParameterMetadata.Add(category, metadata);
                }

                transaction.RollBack();
            }

            return categoryParameterMetadata;
        }

        public static List<Category> GetCategories(Document document, BuiltInParameter builtInParameter, bool inverse)
        {
            var categories = new List<Category>();
            ICollection<ElementId> retResult = new List<ElementId>();
            
            var allCategories = document.Settings.Categories;
            var allFilterableCategories = ParameterFilterUtilities.GetAllFilterableCategories();
            
            foreach( ElementId categoryId in allFilterableCategories)
            {
                var applicableParameters = ParameterFilterUtilities
                    .GetFilterableParametersInCommon(document, new[] { categoryId });
                
                if( applicableParameters.Contains(new ElementId(builtInParameter))) retResult.Add(categoryId);
            }
            
            if(inverse) retResult = allFilterableCategories.Where(elementId => !retResult.Contains(elementId)).ToList();
            
            foreach (Category category in allCategories)
                if (retResult.Any(cat => cat.Equals(category.Id))) categories.Add(category);

            return categories;
        }
        
        public static List<Category> GetCategories(Document document, string parameterName, bool inverse)
        {
            var categories = new List<Category>();
            ICollection<ElementId> retResult = new List<ElementId>();
            
            var allCategories = document.Settings.Categories;
            var allFilterableCategories = ParameterFilterUtilities.GetAllFilterableCategories();

            FilteredElementCollector collector = new FilteredElementCollector(document).WhereElementIsNotElementType();
            var allSharedParameters = collector.OfClass(typeof(SharedParameterElement)).ToList();

            var mainParameter = allSharedParameters.FirstOrDefault(parameter => parameter.Name == parameterName);
            
            foreach (Element e in collector)
            {
                SharedParameterElement param = e as SharedParameterElement;
                Definition def = param.GetDefinition();

                Debug.WriteLine("[" + e.Id + "]\t" + def.Name + "\t(" + param.GuidValue + ")");
            }

            foreach ( ElementId categoryId in allFilterableCategories)
            {
                var applicableParameters = ParameterFilterUtilities
                    .GetFilterableParametersInCommon(document, new[] { categoryId });
                
                if( applicableParameters.Contains(mainParameter.Id)) retResult.Add(categoryId);
            }
            
            if(inverse) retResult = allFilterableCategories.Where(elementId => !retResult.Contains(elementId)).ToList();
            
            foreach (Category category in allCategories)
                if (retResult.Any(cat => cat.Equals(category.Id))) categories.Add(category);

            return categories;
        }
        
        public static SharedParameterElement GetSharedParameter(Document document ,string parameterName)
        {
            var allCategories = document.Settings.Categories;
            var allFilterableCategories = ParameterFilterUtilities.GetAllFilterableCategories();

            FilteredElementCollector collector = new FilteredElementCollector(document).WhereElementIsNotElementType();
            var allSharedParameters = collector.OfClass(typeof(SharedParameterElement)).ToList();

            return (SharedParameterElement) allSharedParameters.FirstOrDefault(parameter => parameter.Name == parameterName);
        }
        
        public static void CopyParameterValue(Parameter source, Parameter target)
        {
            switch (target.StorageType)
            {
                case StorageType.Double:
                    target.Set(source.AsDouble());
                    break;
                case StorageType.Integer:
                    target.Set(source.AsInteger());
                    break;
                case StorageType.String:
                    target.Set(source.AsString());
                    break;
                case StorageType.ElementId:
                    target.Set(source.AsElementId());
                    break;
                case StorageType.None:
                    throw new Exception("Error 'CopyParameterValue'");
            }
        }
    }
}