using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.ModalWindowModule.ViewModel;
using MVVM;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Modules.MVVM
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ThirdPluginCommand : IExternalCommand
    {
        private RevitContextHelper _revitTaskHelper;
        private static void InitializeLogger(Document document)
        {
            var assemblyPath = Path.GetDirectoryName(typeof(ModalWindowViewModel).Assembly.Location);
            var logsPath = $"{assemblyPath}\\Logs";
            var projectName = $"{document.Title}.log";
            var logPath = Path.Combine(logsPath, projectName);

            if (File.Exists(logPath)) File.Delete(logPath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logPath,
                    rollingInterval: RollingInterval.Minute,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            Log.Information("AmethystRed log running...");
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var module = ThirdPluginModule1.GetInstance();
            if (module.IsRunning)
            {
                var msg = "Экземпляр программы уже открыт!";
                var msgTitle = "Ошибка!";

                TaskDialog.Show(msgTitle, msg);

                return Result.Cancelled;
            }

            module.RunControls(commandData);
            _revitTaskHelper = new RevitContextHelper(module.Application);
            InitializeLogger(module.Document);

            try
            {
                // create the view model instance
                var viewModel = new ApplicationViewModel();

                // create the view and pass the view model instance to its constructor
                var view = new MainWindow(viewModel);

                // display the window
                view.Show();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Something wrong");
                TaskDialog.Show($"Ошибка!", exception.Message);
                module.CloseControls();

                return Result.Failed;
            }
            finally
            {
                Log.Information("AmethystRed log closing...");
                Log.CloseAndFlush();

            }

            return Result.Succeeded;

        }
    }
}