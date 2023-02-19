using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.MVVM;
using Modules.ZeroPluginModule.ExternalCommands;
using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Modules.ZeroPluginModule
{
    public class ZeroPluginModule
    {
        public void RunModule(RibbonPanel ribbonPanel)
        {
            var pushButtonName = "Zero mero blyat";
            var pushButtonData = new PushButtonData(nameof(ZeroPluginCommand),
                pushButtonName, AssemblyResourceHelper.GetAssemblyLocation(),
                typeof(ZeroPluginCommand).FullName);

            var largeRibbonImagePath = "Modules\\ZeroPluginModule\\Resources\\Images\\jokeFuck.png";
            var largeRibbonImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(largeRibbonImagePath));

            var toolTipImagePath = "Modules\\ZeroPluginModule\\Resources\\Images\\jokeFuck.png";
            var toolTipImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(toolTipImagePath));

            pushButtonData.LargeImage = largeRibbonImage;
            pushButtonData.ToolTip = "ХУУУУЙ";
            pushButtonData.ToolTipImage = toolTipImage;
            pushButtonData.LongDescription = "ЫЫЫЫЫЫЫЫЫЫЫЫЫ";

            ribbonPanel.AddItem(pushButtonData);

        }

        private static ZeroPluginModule _instance;
        public static ZeroPluginModule GetInstance()
        {
            if (_instance == null)
                _instance = new ZeroPluginModule();
            return _instance;
        }

        private ZeroPluginModule() { }

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

    }
}
