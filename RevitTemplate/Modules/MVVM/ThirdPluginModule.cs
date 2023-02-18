using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.MVVM;

namespace Modules
{
    public class ThirdPluginModule1
    {
        private static ThirdPluginModule1 _instance;
        public static ThirdPluginModule1 GetInstance()
        {
            if (_instance == null)
                _instance = new ThirdPluginModule1();
            return _instance;
        }

        private ThirdPluginModule1() { }

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
            var pushButtonName = "MVVM";
            var pushButtonData = new PushButtonData(nameof(ThirdPluginCommand),
                pushButtonName, AssemblyResourceHelper.GetAssemblyLocation(),
                typeof(ThirdPluginCommand).FullName);

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