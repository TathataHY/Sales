[assembly: Xamarin.Forms.Dependency(typeof(Sales.iOS.Implementations.PathService))]
namespace Sales.iOS.Implementations
{
    using System;
    using System.IO;
    using Interfaces;

    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string docfolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libfolder = Path.Combine(docfolder, "..", "Library", "Databases");

            if (!Directory.Exists(libfolder))
            {
                Directory.CreateDirectory(libfolder);
            }

            return Path.Combine(libfolder, "Sales.db3");
        }
    }
}