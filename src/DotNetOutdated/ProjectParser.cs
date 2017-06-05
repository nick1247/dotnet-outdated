using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml;
using System;

namespace DotNetOutdated
{
    public static class ProjectParser
    {
        public static IEnumerable<Dependency> GetAllDependencies(string filePath)
        {
            HashSet<Dependency> all = new HashSet<Dependency>();
            var csproj = new XmlDocument();
            csproj.LoadXml(File.ReadAllText(filePath));

            var nodes = csproj.GetElementsByTagName("ItemGroup");
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes.Item(i);
                if (node.HasChildNodes)
                {
                    for (int j = 0; j < node.ChildNodes.Count; j++)
                    {
                        var childNode = node.ChildNodes.Item(j);
                        var dependency = _extract(childNode);

                        if (dependency != null) all.Add(dependency);
                    }
                }
            }

            return all;
        }

        private static Dependency _extract(XmlNode node)
        {
            if (string.CompareOrdinal(node.Name, "PackageReference") != 0 && string.CompareOrdinal(node.Name, "DotNetCliToolReference") != 0) return null;

            var name = node.Attributes["Include"].Value;
            var version = node.Attributes["Version"].Value;

            return new Dependency(name, version);
        }
    }
}