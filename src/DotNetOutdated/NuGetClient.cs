using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using NuGet;

namespace DotNetOutdated
{
    public abstract class NuGetClient
    {
        protected abstract Task<JObject> GetResource(string name);

        public PackageInfo GetPackageInfo(string packageName)
        {
            var repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var packages = repo.FindPackagesById(packageName).ToList();

            // var json = await this.GetResource($"{packageName.ToLower()}/index.json");
            // var versions = new List<SemanticVersion>();

            // var items = json["items"].AsJEnumerable();
            // if (items.Count() == 1)
            //     versions.AddRange(this.ExtractVersions(items.ElementAt(0)["items"]));
            // else 
            // {
            //     var requests = items.Select(i => {
            //         var id = i["@id"].ToString();
            //         var resourceName = id.Substring(id.IndexOf(packageName.ToLower()));
            //         return this.GetResource(resourceName);
            //     });
                
            //     var pages = await Task.WhenAll(requests);
            //     foreach(var page in pages) 
            //         versions.AddRange(this.ExtractVersions(page["items"]));
            // }

            packages.Reverse();
            return new PackageInfo(packageName, packages);
        }

        private IEnumerable<SemanticVersion> ExtractVersions(JToken items)
        {
            foreach (var item in items)
            {
                bool listed = Convert.ToBoolean(item["catalogEntry"]["listed"].ToString());
                if (!listed)
                    continue;

                SemanticVersion version;
                if (SemanticVersion.TryParse(item["catalogEntry"]["version"].ToString(), out version))
                    yield return version;
            }
        }
    }
}