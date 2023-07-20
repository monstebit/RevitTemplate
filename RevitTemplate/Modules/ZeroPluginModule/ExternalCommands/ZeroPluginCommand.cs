using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.ZeroPluginModule.View;
using Modules.ZeroPluginModule.ViewModel;
using System.Windows;
using System.Windows.Forms;

namespace Modules.ZeroPluginModule.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ZeroPluginCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication application = commandData.Application;
            UIDocument uiDocument = application.ActiveUIDocument;
            Document document = uiDocument.Document;

            System.Windows.Forms.MessageBox.Show("Я запустил начало создания окна ZeroPluginModule", "text", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var module = ZeroPluginModule.GetInstance();

            module.RunControls(commandData);

            var contextHelper = new RevitContextHelper(application);
            var myViewModel = new ZeroPluginViewModel(contextHelper);
            var myView = new ZeroPluginView(myViewModel);

            myView.Show();
            return Result.Succeeded;
        }
    }
}


