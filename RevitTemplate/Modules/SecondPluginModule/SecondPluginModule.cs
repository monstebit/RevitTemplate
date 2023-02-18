using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.SecondPluginModule.ExternalCommands;

namespace Modules
{
    public class SecondPluginModule1
    {
        private static SecondPluginModule1 _instance;
        public static SecondPluginModule1 GetInstance()
        {
            if (_instance == null)
                _instance = new SecondPluginModule1();
            return _instance;
        }

        private SecondPluginModule1() { }

        public ExternalCommandData CommandData;
        public Document Document;
        public UIApplication Application;
        public UIDocument UiDocument;
        public bool IsRunning;

        public void RunControls(ExternalCommandData commandData)
        {
            Application = commandData.Application;
            UiDocument = Application.ActiveUIDocument;
            Document = UiDocument.Document;
            IsRunning = true;
        }

        public void CloseControls()
        {
            Application = null;
            UiDocument = null;
            Document = null;
            IsRunning = false;
        }

        public void RunModule(RibbonPanel ribbonPanel)
        {
            var pushButtonName = "Set Param\nValue";
            var pushButtonData = new PushButtonData(nameof(SecondPluginCommand),
                pushButtonName, AssemblyResourceHelper.GetAssemblyLocation(),
                typeof(SecondPluginCommand).FullName);

            var largeRibbonImagePath = "Modules\\SecondPluginModule\\Resources\\Images\\lsrSetParamValue.png";
            var largeRibbonImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(largeRibbonImagePath));

            var toolTipImagePath = "Modules\\SecondPluginModule\\Resources\\Images\\lsrSetParamValue.png";
            var toolTipImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(toolTipImagePath));

            pushButtonData.LargeImage = largeRibbonImage;
            pushButtonData.ToolTip = "//";
            pushButtonData.ToolTipImage = toolTipImage;
            pushButtonData.LongDescription = "//";

            ribbonPanel.AddItem(pushButtonData);

        }
    }
}