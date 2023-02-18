using System;
using System.Reflection;

namespace Core.Helpers
{
    public static class AssemblyResourceHelper
    {
        public static Uri GetUriResource(string path)
        {
            var assemblyName = GetAssemblyName();
            return new Uri($"pack://application:,,,/{assemblyName};component/{path}");
        }

        public static string GetAssemblyName()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyName = AssemblyName.GetAssemblyName(assemblyLocation);
            return assemblyName.Name;
        }
        
        public static string GetAssemblyLocation() => Assembly.GetExecutingAssembly().Location;
    }
}