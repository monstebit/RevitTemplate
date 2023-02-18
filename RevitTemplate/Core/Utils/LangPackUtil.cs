using System.Globalization;
using System.IO;
using System.Resources;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Core.Utils
{
    public static class LangPackUtil
    {
        private static ResourceManager _languageResources;
        private static CultureInfo _cultureInfo;
        
        public static void GetLocalisationValues(UIControlledApplication application)
        {
            var lang = application.ControlledApplication.Language.ToString();
            
            if (lang == "Russian")
            {
                _cultureInfo = CultureInfo.CreateSpecificCulture("ru");
                _languageResources = new ResourceManager("Resources.LangPack.RusLang", 
                    System.Reflection.Assembly.GetExecutingAssembly());
            }
            else
            {
                _cultureInfo = CultureInfo.CreateSpecificCulture("en");
                _languageResources = new ResourceManager("Resources.LangPack.EngLang", 
                    System.Reflection.Assembly.GetExecutingAssembly());
            }
        }

        public static ResourceManager GetLanguageResources() => _languageResources;
        public static CultureInfo GetCultureInfo() => _cultureInfo;
    }
}
