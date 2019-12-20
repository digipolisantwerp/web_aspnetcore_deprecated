using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Digipolis.Web.Api.Tools
{
    public static class AssemblyExtensions
    {
        public static string[] GetXmlDocPaths(this IEnumerable<Assembly> assemblies)
        {
            var assemblyLocations = new List<string>();
            foreach (var assembly in assemblies)
                assemblyLocations.AddRange(GetAssemblyLocations(assembly));

            return assemblyLocations.ToArray();
        }

        private static IEnumerable<string> GetAssemblyLocations(this Assembly assembly)
        {
            var assemblyNames = assembly.GetReferencedAssemblies().Union(new[] {assembly.GetName()});

            var xmlPaths = assemblyNames.Select(a => Path.Combine(Path.GetDirectoryName(assembly.Location), $"{a.Name}.xml")).Where(File.Exists);

            return xmlPaths;
        }
    }
}