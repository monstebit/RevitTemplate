using System;
using System.Linq;
using System.Windows.Media;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using Core.Utils;
using Modules;
using Modules.FirstPluginModule;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;
using Modules.ZeroPluginModule;
using Modules.ThirdPluginModule;
using Autodesk.Revit.DB;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using Autodesk.Revit.DB.Events;
using Color = System.Windows.Media.Color;
using Autodesk.Revit.DB.Mechanical;
using System.IO;
using Serilog;
using System.Windows.Documents;
using Autodesk.Revit.ApplicationServices;
using Modules.ModalWindowModule;

//public class MyUpdater : IUpdater
//{
//    public static AddInId _addinId = new AddInId(new Guid("fa77811b-0acf-4839-bbc6-71d709efd405"));

//    public static UpdaterId _updaterId = new UpdaterId(_addinId, new Guid("e081d26c-713b-4f9a-afc7-d07a17013c49"));

//    public string GetAdditionalInformation() => "MyUpdater";

//    public ChangePriority GetChangePriority() => ChangePriority.Annotations;

//    public UpdaterId GetUpdaterId() => _updaterId;

//    public string GetUpdaterName() => "My Updater";

//    public void Execute(UpdaterData data)
//    {
//        var addedElements = data.GetAddedElementIds();
//        var deletedElements = data.GetDeletedElementIds();
//        var modifyElements = data.GetModifiedElementIds();


//    }
//}

public class Application : IExternalApplication
{
    private const string TabPanelName = "LSR BIM"; 

    private const string RibbonPanel1Name = "PLUGINPANEL1";
    private const string RibbonPanel1Title = "MEP";

    //private const string RibbonPanel2Name = "PLUGINPANEL2";
    //private const string RibbonPanel2Title = "**";

    //private const string RibbonPanel3Name = "PLUGINPANEL3";
    //private const string RibbonPanel3Title = "***";

    //private const string RibbonPanel4Name = "PLUGINPANEL4";
    //private const string RibbonPanel4Title = "****";

    private RibbonTab _ribbonTab;
        
    private RibbonPanel _ribbonPanel1;
    //private RibbonPanel _ribbonPanel2;
    private RibbonPanel _ribbonPanel3;
    //private RibbonPanel _ribbonPanel4;

    //private MyUpdater _updater;

    // Обработчик событий
    public Result OnStartup(UIControlledApplication uiControlledApplication)
    {
        LangPackUtil.GetLocalisationValues(uiControlledApplication);

        _ribbonTab = InitRibbonTab(uiControlledApplication);
        _ribbonPanel1 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel1Name,RibbonPanel1Title);
        //_ribbonPanel2 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel2Name,RibbonPanel2Title);
        //_ribbonPanel3 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel3Name, RibbonPanel3Title);
        //_ribbonPanel4 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel4Name,RibbonPanel4Title);

        InitRibbonPanelsTheme();

        //System.Windows.Forms.MessageBox.Show("Опа я запустил начало создания окна", "Йоу", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //var modalWindowModule = ModalWindowModule.GetInstance();
        //modalWindowModule.RunModule(_ribbonPanel1);

        var firstModule = FirstPluginModule1.GetInstance();
        firstModule.RunModule(_ribbonPanel1);

        //var secondModule = SecondPluginModule1.GetInstance();
        //secondModule.RunModule(_ribbonPanel2);

        //var zeroPluginModule = ZeroPluginModule.GetInstance();
        //zeroPluginModule.RunModule(_ribbonPanel1);

        var thirdPluginModule = ThirdPluginModule.GetInstance();
        thirdPluginModule.RunModule(_ribbonPanel1);


        //_updater = new MyUpdater();
        //UpdaterRegistry.RegisterUpdater(_updater, true);

        //// Change Scope = any Wall element`
        //ElementClassFilter wallFilter = new ElementClassFilter(typeof(Duct));

        //// Change type = element addition
        //UpdaterRegistry.AddTrigger(_updater.GetUpdaterId(), wallFilter,
        //                            Element.GetChangeTypeAny());

        // Регистрация ивента MyCustomMethod
        // в обработчике событий
        //var m_CtrlApp = uiControlledApplication.ControlledApplication;
        //m_CtrlApp.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(MyCustomMethod);

        // Регистрация ивента OnDocumentSynchronizedWithCentral
        // в обработчике событий
        //m_CtrlApp.DocumentSynchronizedWithCentral += OnDocumentSynchronizedWithCentral;

        return Result.Succeeded;
    }

    //private void MyCustomMethod(object sender, DocumentChangedEventArgs e)
    //{
    //    string LogFilePath = @"C:\Coding\C_Sharp\01_RevitJournal\MyJournals\Test\log.txt";
    //    //string LogFilePath = @"\\lsr.ru\dfs-bim\Project_Spb\ЦГ-7\00_Информация\07_Скрипты\48_Логи транзакций\00_Файлы логов\log.txt";

    //    //try
    //    //{
    //    //    // Try to access the file at the given path
    //    //    File.AppendAllText(LogFilePath, "Test log entry");
    //    //    // Открывает файл, добавляет в него указанную строку и затем закрывает файл.
    //    //    // Если файл не существует, этот метод создает файл,
    //    //    // записывает в него указанную строку и затем закрывает файл.
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    // If an exception is thrown, show a task dialog with an error message
    //    //    TaskDialog.Show("Error", $"There was an error accessing the log file:\n{ex.Message}");
    //    //}

    //    // Get the current username
    //    string username = Environment.UserName;

    //    // Create a log of changes
    //    //string log = $"User {username} время {DateTime.Now.ToString()}:\n";

    //    // Эта строка кода проверяет, существует ли файл в месте,
    //    // указанном переменной LogFilePath. Если файл существует,
    //    // он считывает все его содержимое в log переменную.
    //    // Если файл не существует, переменной присваивается пустая строка log.
    //    string log = File.Exists(LogFilePath) ? File.ReadAllText(LogFilePath) : "";

    //    var addedElements = e.GetAddedElementIds();
    //    var modifyElements = e.GetModifiedElementIds();
    //    var deletedElements = e.GetDeletedElementIds();

    //    // If no changes were made, ignore the transaction
    //    if (addedElements.Count == 0 && modifyElements.Count == 0 && deletedElements.Count == 0)
    //    {
    //        return;
    //    }

    //    var transactionNames = e.GetTransactionNames();
        
    //    var doc = e.GetDocument();
    //    var docName = e.GetDocument().Title;

    //    // Append the new log information to the existing log
    //    log = $"\nFile: {docName}\nUser: {username}\nDate: {DateTime.Now.ToString()}\n";

    //    foreach (var transaction in transactionNames)
    //    {
    //        var transactionName = transaction.ToString();

    //        log += $"Transaction: {transactionName}\n";
    //    }

    //    // Add the added elements to the log
    //    foreach (ElementId id in addedElements)
    //    {
    //        Element addedElem = doc.GetElement(id);

    //        string addedElemCategory = addedElem.Category != null ? addedElem.Category.Name : "Null";
    //        string addedElemName = addedElem.Name != "" ? addedElem.Name : "Null";

    //        log += $"Added element: {addedElem.Name} | ID - {id} | Category - {addedElemCategory}\n";
    //    }

    //    // Add the modified elements to the log
    //    foreach (ElementId id in modifyElements)
    //    {
    //        Element modifyElem = doc.GetElement(id);

    //        string modifyElemCategory = modifyElem.Category != null ? modifyElem.Category.Name : "Null";
    //        string modifyElemName = modifyElem.Name != "" ? modifyElem.Name : "Null";

    //        log += $"Modified element: {modifyElemName} | ID - {id} | Category - {modifyElemCategory}\n";
    //    }

    //    // Add the deleted elements to the log
    //    foreach (ElementId id in deletedElements)
    //    {
    //        Element deletedElem = doc.GetElement(id);;

    //        log += $"Deleted element: {deletedElem} | ID - {id}\n";
    //    }

    //    // Write the log to a file
    //    string logFilePath = @"C:\Coding\C_Sharp\01_RevitJournal\MyJournals\Test\log.txt";
    //    //string logFilePath = @"\\lsr.ru\dfs-bim\Project_Spb\ЦГ-7\00_Информация\07_Скрипты\48_Логи транзакций\00_Файлы логов\log.txt";

    //    // Метод для добавления log содержимого в конец файла по адресу logFilePath.
    //    // Это означает, что любой существующий контент в файле останется,
    //    // а новый контент будет добавлен в конец файла.
    //    File.AppendAllText(logFilePath, log);

    //    //// Write the updated log to the log file
    //    /// File.WriteAllTextметод для перезаписи всего файла LogFilePathновым logсодержимым. 
    //    /// Это означает, что любой существующий контент в файле будет заменен новым контентом.
    //    //File.WriteAllText(LogFilePath, log);

    //    //// Define the paths of the source and destination files
    //    //string sourcePath = @"C:\Coding\Tests\log.txt";
    //    //string destinationPath = @"\\lsr.ru\dfs-bim\Project_Spb\ЦГ-7\02_Сводная модель\log.txt";

    //    //// Copy the file
    //    //File.Copy(sourcePath, destinationPath);
    //}

    //private void OnDocumentSynchronizedWithCentral(object sender, DocumentSynchronizedWithCentralEventArgs e)
    //{
    //    var doc = e.Document;

    //    string LogFilePath = @"C:\Coding\C_Sharp\01_RevitJournal\MyJournals\Test\logSynchronized.txt";
    //    //string LogFilePath = @"\\lsr.ru\dfs-bim\Project_Spb\ЦГ-7\00_Информация\07_Скрипты\48_Логи транзакций\00_Файлы логов\logSynchronized.txt";
    //    string username = doc.Application.Username;
    //    string fileName = doc.Title;

    //    string log = File.Exists(LogFilePath) ? File.ReadAllText(LogFilePath) : "";
    //    log = $"\nFile: {fileName}\nUser: {username}\nDate: {DateTime.Now.ToString()}\n";

    //    string logFilePath = @"C:\Coding\C_Sharp\01_RevitJournal\MyJournals\Test\logSynchronized.txt";

    //    File.AppendAllText(logFilePath, log);
    //}

    public Result OnShutdown(UIControlledApplication application)
    {
        // Unregister the updater
        //UpdaterRegistry.UnregisterUpdater(_updater.GetUpdaterId());

        // Unregister the MyCustomMethod
        //var m_CtrlApp = application.ControlledApplication;
        //m_CtrlApp.DocumentChanged -= MyCustomMethod;

        // Unregister the OnDocumentSynchronizedWithCentral
        //application.ControlledApplication.DocumentSynchronizedWithCentral -= OnDocumentSynchronizedWithCentral;

        return Result.Succeeded;
    }

    private void InitRibbonPanelsTheme()
    {
        var ribbonPanel1Gradient = new LinearGradientBrush();
        var ribbonPanel1GradientColor1 = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        var ribbonPanel1TextBlockColor = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        ribbonPanel1Gradient.StartPoint = new System.Windows.Point( 0, 0 );
        ribbonPanel1Gradient.EndPoint = new System.Windows.Point( 0, 1 );
        ribbonPanel1Gradient.GradientStops.Add( 
            new GradientStop( Colors.White, 0.0 ) );
        ribbonPanel1Gradient.GradientStops.Add( 
            new GradientStop( ribbonPanel1GradientColor1, 2 ) );
        var ribbonPanel1BackGroundColor = new SolidColorBrush(ribbonPanel1TextBlockColor);

        //var ribbonPanel2Gradient = new LinearGradientBrush();
        //var ribbonPanel2GradientColor1 = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        //var ribbonPanel2TextBlockColor = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        //ribbonPanel2Gradient.StartPoint = new System.Windows.Point( 0, 0 );
        //ribbonPanel2Gradient.EndPoint = new System.Windows.Point( 0, 1 );
        //ribbonPanel2Gradient.GradientStops.Add( 
        //    new GradientStop( Colors.White, 0.0 ) );
        //ribbonPanel2Gradient.GradientStops.Add( 
        //    new GradientStop( ribbonPanel2GradientColor1, 2 ) );
        //var ribbonPanel2BackGroundColor = new SolidColorBrush(ribbonPanel2TextBlockColor);


        var ribbonPanel3Gradient = new LinearGradientBrush();
        var ribbonPanel3GradientColor1 = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        var ribbonPanel3TextBlockColor = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        ribbonPanel3Gradient.StartPoint = new System.Windows.Point(0, 0);
        ribbonPanel3Gradient.EndPoint = new System.Windows.Point(0, 1);
        ribbonPanel3Gradient.GradientStops.Add(
            new GradientStop(Colors.White, 0.0));
        ribbonPanel3Gradient.GradientStops.Add(
            new GradientStop(ribbonPanel3GradientColor1, 2));
        var ribbonPanel3BackGroundColor = new SolidColorBrush(ribbonPanel3TextBlockColor);

        //var ribbonPanel4Gradient = new LinearGradientBrush();
        //var ribbonPanel4GradientColor1 = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        //var ribbonPanel4TextBlockColor = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        //ribbonPanel4Gradient.StartPoint = new System.Windows.Point( 0, 0 );
        //ribbonPanel4Gradient.EndPoint = new System.Windows.Point( 0, 1 );
        //ribbonPanel4Gradient.GradientStops.Add( 
        //    new GradientStop( Colors.White, 0.0 ) );
        //ribbonPanel4Gradient.GradientStops.Add( 
        //    new GradientStop( ribbonPanel4GradientColor1, 2 ) );
        //var ribbonPanel4BackGroundColor = new SolidColorBrush(ribbonPanel4TextBlockColor);

        foreach (var panel in _ribbonTab.Panels )
        {
            var ribbonSource = panel.Source;
            switch (ribbonSource.Name)
            {
                case RibbonPanel1Name:
                    panel.CustomPanelTitleBarBackground = ribbonPanel1BackGroundColor;
                    panel.CustomPanelBackground = ribbonPanel1Gradient;
                    panel.CustomSlideOutPanelBackground = ribbonPanel1BackGroundColor;
                    break;

                //case RibbonPanel2Name:
                //    panel.CustomPanelTitleBarBackground = ribbonPanel2BackGroundColor;
                //    panel.CustomPanelBackground = ribbonPanel2Gradient;
                //    panel.CustomSlideOutPanelBackground = ribbonPanel2BackGroundColor;
                //    break;

                //case RibbonPanel3Name:
                //    panel.CustomPanelTitleBarBackground = ribbonPanel3BackGroundColor;
                //    panel.CustomPanelBackground = ribbonPanel3Gradient;
                //    panel.CustomSlideOutPanelBackground = ribbonPanel3BackGroundColor;
                //    break;

                    //case RibbonPanel4Name:
                    //    panel.CustomPanelTitleBarBackground = ribbonPanel4BackGroundColor;
                    //    panel.CustomPanelBackground = ribbonPanel4Gradient;
                    //    panel.CustomSlideOutPanelBackground = ribbonPanel4BackGroundColor;
                    //    break;
            }
        }
    }

    private static RibbonPanel InitRibbonPanel(UIControlledApplication application, RibbonTab ribbonTab, string panelName,string panelTittle)
    {
        RibbonPanel ribbonPanel = null;
        foreach (var internalRibbonPanel in ribbonTab.Panels)
        {
            var ribbonSource = internalRibbonPanel.Source;
            if (ribbonSource.Name != panelName) continue;
            ribbonPanel = application.GetRibbonPanels(ribbonTab.Name)
                .FirstOrDefault(name => name.Name == panelName);
            break;
        }

        if (ribbonPanel is null)
        {
            ribbonPanel = application.CreateRibbonPanel(TabPanelName, panelName);
            ribbonPanel.Title = panelTittle;
        }

        return ribbonPanel;
    }

    private static RibbonTab InitRibbonTab(UIControlledApplication application)
    {
        var ribbonControlTabs = ComponentManager.Ribbon.Tabs;
        var ribbonTab = ribbonControlTabs.FirstOrDefault(tab => tab.Name == TabPanelName);

        if (ribbonTab is null)
        {
            application.CreateRibbonTab(TabPanelName);
            ribbonTab = ribbonControlTabs.FirstOrDefault(tab => tab.Name == TabPanelName);
        }

        return ribbonTab;
    }

    //internal Autodesk.Revit.DB.Document Open(string v)
    //{
    //    throw new NotImplementedException();
    //}
}