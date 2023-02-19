using Autodesk.Revit.DB;
using Core.Enums;
using Core.Results;
using Modules.ZeroPluginModule.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ZeroPluginModule.Core
{
    public class RevitSelector
    {
        public ZeroPluginViewModel ViewModel { get; set; }
        public RevitSelector(ZeroPluginViewModel viewModel) 
        {
            ViewModel = viewModel;
        }

        public void UpdateSelectedElementsToViewModel()
        {
            var module = ZeroPluginModule.GetInstance();
            var document = module.Document;
            var uiDocument = module.UiDocument;

            var selectedElementIds = uiDocument.Selection.GetElementIds()
                .Select(elementId => document.GetElement(elementId))
                .ToList();

            ViewModel.SelectedElements = new ObservableCollection < Element > (selectedElementIds);
        }
        public RevitContextResult ChangeCommentParameterValue()
        {
            var element = ViewModel.SelectedElement;
            var stringValueForParameter = ViewModel.CommentString;
            var module = ZeroPluginModule.GetInstance();
            var document = module.Document;


            var parameterComment = element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            if (parameterComment == null) return new RevitContextResult(ResultStatus.Error,
                "Параметр комментарий не найден дебил");
            ;

            using (var tr = new Transaction(document))
            {
                tr.Start("Startttt");

                parameterComment.Set(stringValueForParameter);

                tr.Commit();
            }
            return new RevitContextResult(ResultStatus.Complete,
            "Все заебок");

        }
    }
}
