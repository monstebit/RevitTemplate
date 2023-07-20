using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Modules.ModalWindowModule.View;
using Modules.ModalWindowModule.ViewModel;
using Modules.ThirdPluginModule.View;
using Modules.ThirdPluginModule.ViewModel;
using Serilog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Modules.ThirdPluginModule.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ThirdPluginCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication application = commandData.Application;
            UIDocument uiDocument = application.ActiveUIDocument;
            Document document = uiDocument.Document;

            //System.Windows.Forms.MessageBox.Show("Я запустил начало создания окна ThirdPluginModule", "text", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var module = ThirdPluginModule.GetInstance();

            if (module.IsRunning)
            {
                var msg = "Экземпляр программы уже открыт!";
                var msgTitle = "Ошибка!";

                TaskDialog.Show(msgTitle, msg);

                return Result.Cancelled;
            }

            module.RunControls(commandData);

            var contextHelper = new RevitContextHelper(application);
            var myViewModel = new ThirdPluginViewModel(contextHelper);
            var myView = new ThirdPluginView(myViewModel);

            myView.Show();
            return Result.Succeeded;
        }
    }
}


