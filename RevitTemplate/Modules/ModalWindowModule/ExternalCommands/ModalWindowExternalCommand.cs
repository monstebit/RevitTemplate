using System;
using System.IO;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.ModalWindowModule.View;
using Modules.ModalWindowModule.ViewModel;
using Serilog;

namespace Modules.ModalWindowModule.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ModalWindowExternalCommand : IExternalCommand
    {
        private RevitContextHelper _revitTaskHelper;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var module = ModalWindowModule.GetInstance();
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
                var mainViewModel = new ModalWindowViewModel(_revitTaskHelper);
                var mainView = new ModalWindowView(mainViewModel);
                mainView.Show();
            
                return Result.Succeeded;
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
        }

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
    }
}
