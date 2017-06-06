using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace DotNetOutdated
{
    public class PackageInfo
    {
        public string Name { get; private set; }
        public IEnumerable<IPackage> Versions { get; private set; }

        public PackageInfo(string name, IEnumerable<IPackage> versions)
        {
            this.Name = name;
            this.Versions = versions;
        }
    }
}