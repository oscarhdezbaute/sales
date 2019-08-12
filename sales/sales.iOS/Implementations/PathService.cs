[assembly: Xamarin.Forms.Dependency(typeof(Sales.iOS.Implementations.PathService))]

namespace Sales.iOS.Implementations
{
    using System;
    using System.IO;
    using sales.Interfaces;

    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, "Sales.db3");
        }
    }
}
