using System;
using System.Windows;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using Core.Helpers;
using Core.Utils;
using Modules;
using Modules.FirstPluginModule;
using Modules.FirstPluginModule.ExternalCommands;
using Modules.ModalWindowModule;
using Modules.ModalWindowModule.ExternalCommands;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;
using Modules.ZeroPluginModule;
using System.Windows.Forms;

public class Application : IExternalApplication
{
    private const string TabPanelName = "LSR BIM"; // before Chamion Revit Template

    private const string RibbonPanel1Name = "PLUGINPANEL1";
    private const string RibbonPanel1Title = "*";

    private const string RibbonPanel2Name = "PLUGINPANEL2";
    private const string RibbonPanel2Title = "**";

    private const string RibbonPanel3Name = "PLUGINPANEL3";
    private const string RibbonPanel3Title = "***";

    //private const string RibbonPanel4Name = "PLUGINPANEL4";
    //private const string RibbonPanel4Title = "****";

    private RibbonTab _ribbonTab;
        
    private RibbonPanel _ribbonPanel1;
    private RibbonPanel _ribbonPanel2;
    private RibbonPanel _ribbonPanel3;
    //private RibbonPanel _ribbonPanel4;

    public Result OnStartup(UIControlledApplication uiControlledApplication)
    {
        LangPackUtil.GetLocalisationValues(uiControlledApplication);
        _ribbonTab = InitRibbonTab(uiControlledApplication);
        _ribbonPanel1 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel1Name,RibbonPanel1Title);
        _ribbonPanel2 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel2Name,RibbonPanel2Title);
        _ribbonPanel3 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel3Name, RibbonPanel3Title);
        //_ribbonPanel4 = InitRibbonPanel(uiControlledApplication, _ribbonTab, RibbonPanel4Name,RibbonPanel4Title);
        InitRibbonPanelsTheme();
        //System.Windows.Forms.MessageBox.Show("Опа я запустил начало создания окна", "Йоу", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //var modalWindowModule = ModalWindowModule.GetInstance();
        //modalWindowModule.RunModule(_ribbonPanel1);

        var firstModule = FirstPluginModule1.GetInstance();
        firstModule.RunModule(_ribbonPanel1);

        var secondModule = SecondPluginModule1.GetInstance();
        secondModule.RunModule(_ribbonPanel2);

        var thirddModule = ThirdPluginModule1.GetInstance();
        thirddModule.RunModule(_ribbonPanel3);

        var zeroPluginModule = ZeroPluginModule.GetInstance();
        zeroPluginModule.RunModule(_ribbonPanel1);



        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
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
        
        var ribbonPanel2Gradient = new LinearGradientBrush();
        var ribbonPanel2GradientColor1 = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        var ribbonPanel2TextBlockColor = (Color)ColorConverter.ConvertFromString("#E1A4AF");
        ribbonPanel2Gradient.StartPoint = new System.Windows.Point( 0, 0 );
        ribbonPanel2Gradient.EndPoint = new System.Windows.Point( 0, 1 );
        ribbonPanel2Gradient.GradientStops.Add( 
            new GradientStop( Colors.White, 0.0 ) );
        ribbonPanel2Gradient.GradientStops.Add( 
            new GradientStop( ribbonPanel2GradientColor1, 2 ) );
        var ribbonPanel2BackGroundColor = new SolidColorBrush(ribbonPanel2TextBlockColor);


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

                case RibbonPanel2Name:
                    panel.CustomPanelTitleBarBackground = ribbonPanel2BackGroundColor;
                    panel.CustomPanelBackground = ribbonPanel2Gradient;
                    panel.CustomSlideOutPanelBackground = ribbonPanel2BackGroundColor;
                    break;

                case RibbonPanel3Name:
                    panel.CustomPanelTitleBarBackground = ribbonPanel3BackGroundColor;
                    panel.CustomPanelBackground = ribbonPanel3Gradient;
                    panel.CustomSlideOutPanelBackground = ribbonPanel3BackGroundColor;
                    break;

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

    internal Autodesk.Revit.DB.Document Open(string v)
    {
        throw new NotImplementedException();
    }
}