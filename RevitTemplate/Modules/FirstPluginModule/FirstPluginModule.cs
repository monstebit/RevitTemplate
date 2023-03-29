using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.FirstPluginModule.ExternalCommands;
using System.Windows.Media.Imaging;

namespace Modules.FirstPluginModule
{
    public class FirstPluginModule1
    {
        private static FirstPluginModule1 _instance;
        public static FirstPluginModule1 GetInstance()
        {
            if (_instance == null)
                _instance = new FirstPluginModule1();
            return _instance;
        }

        private FirstPluginModule1() { }

        public void RunModule(RibbonPanel ribbonPanel)
        {
            var pushButtonName = "MEP Scheme";
            var pushButtonData = new PushButtonData(nameof(FirstPluginCommand),
                pushButtonName, AssemblyResourceHelper.GetAssemblyLocation(),
                typeof(FirstPluginCommand).FullName);

            var largeRibbonImagePath = "Modules\\FirstPluginModule\\Resources\\Images\\lsrMepScheme2.png";
            var largeRibbonImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(largeRibbonImagePath));

            var toolTipImagePath = "Modules\\FirstPluginModule\\Resources\\Images\\lsrMepScheme2.png";
            var toolTipImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(toolTipImagePath));

            pushButtonData.LargeImage = largeRibbonImage;
            pushButtonData.ToolTip = "Автоматическое создание видов с изометрией MEP систем.";
            pushButtonData.ToolTipImage = toolTipImage;
            pushButtonData.LongDescription = "...";

            ribbonPanel.AddItem(pushButtonData);
        }
    }
}