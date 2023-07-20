using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.ThirdPluginModule.ExternalCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Modules.ThirdPluginModule
{
    public class ThirdPluginModule
    {
        public void RunModule(RibbonPanel ribbonPanel)
        {
            var pushButtonName = "Element Level\nChanger";
            var pushButtonData = new PushButtonData(nameof(ThirdPluginCommand),
                pushButtonName, AssemblyResourceHelper.GetAssemblyLocation(),
                typeof(ThirdPluginCommand).FullName);

            var largeRibbonImagePath = "Modules\\ThirdPluginModule\\Resources\\Images\\selectLevelElems.png";                                    
            var largeRibbonImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(largeRibbonImagePath));

            var toolTipImagePath = "Modules\\ThirdPluginModule\\Resources\\Images\\selectLevelElems.png";
            var toolTipImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(toolTipImagePath));

            pushButtonData.LargeImage = largeRibbonImage;
            pushButtonData.ToolTip = "Изменения уровня для выделенных MEP элементов БЕЗ смещения в модели.";
            pushButtonData.ToolTipImage = toolTipImage;
            pushButtonData.LongDescription = "...";

            ribbonPanel.AddItem(pushButtonData);

        }

        private static ThirdPluginModule _instance;
        public static ThirdPluginModule GetInstance()
        {
            if (_instance == null)
                _instance = new ThirdPluginModule();
            return _instance;
        }

        private ThirdPluginModule() { }

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
