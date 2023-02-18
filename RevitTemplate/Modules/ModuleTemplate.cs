namespace Modules
{
    public class TemplateModule
    {
        private static TemplateModule _instance;
        public static TemplateModule GetInstance()
        {
            if (_instance == null)
                _instance = new TemplateModule();
            return _instance;
        }

        private TemplateModule() { }
        
        public void RunModule()
        {

        }
    }
}