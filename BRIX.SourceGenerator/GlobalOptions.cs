using Microsoft.CodeAnalysis.Diagnostics;

namespace BRIX.SourceGenerator
{
    public class GlobalOptions
    {
        public GlobalOptions(AnalyzerConfigOptions options)
        {
            options.TryGetValue("build_property.projectdir", out var projectFullPath);
            ProjectFullPath = projectFullPath!;

            options.TryGetValue("build_property.rootnamespace", out var rootNamespace);
            RootNamespace = rootNamespace!;
        }

        public string RootNamespace { get; }
        public string ProjectFullPath { get; }
    }
}
