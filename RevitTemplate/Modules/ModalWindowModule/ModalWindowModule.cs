using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Core.Helpers;
using Modules.ModalWindowModule.ExternalCommands;

namespace Modules.ModalWindowModule
{
    public class ModalWindowModule
    {
        private static ModalWindowModule _instance;
        public static ModalWindowModule GetInstance()
        {
            if (_instance == null)
                _instance = new ModalWindowModule();
            return _instance;
        }

        private ModalWindowModule() { }
        
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

        //public void RunModule(RibbonPanel ribbonPanel)
        //{
        //    var pushButtonName = "ModalWindow";
        //    var pushButtonData = new PushButtonData(nameof(ModalWindowExternalCommand),
        //        pushButtonName, AssemblyResourceHelper.GetAssemblyLocation(),
        //        typeof(ModalWindowExternalCommand).FullName);

        //    var largeRibbonImagePath = "Modules/ModalWindowModule/Resources/Images/Frames.png";
        //    var largeRibbonImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(largeRibbonImagePath));

        //    var toolTipImagePath = "Modules/ModalWindowModule/Resources/Images/ChamionLogo150x150.png";
        //    var toolTipImage = new BitmapImage(AssemblyResourceHelper.GetUriResource(toolTipImagePath));

        //    pushButtonData.LargeImage = largeRibbonImage;
        //    pushButtonData.ToolTip = "Описание плагина. Этот плагин САМЫЙ ЛУЧШИЙ. Лучше только Я!";
        //    pushButtonData.ToolTipImage = toolTipImage;
        //    pushButtonData.LongDescription = "А вот это ОЧЕНЬ ДЛИННОЕ ОПИСАНИЕ.";

        //    ribbonPanel.AddItem(pushButtonData);
        //}       
    }
}