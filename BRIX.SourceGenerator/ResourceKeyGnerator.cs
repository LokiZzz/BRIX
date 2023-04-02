using Microsoft.CodeAnalysis;
using System.Linq;
using System.Collections.Immutable;

namespace BRIX.SourceGenerator
{
    [Generator]
    public class ResourceKeyGnerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValueProvider<GlobalOptions> globalOptions = 
                context
                .AnalyzerConfigOptionsProvider
                .Select((provider, ct) => new GlobalOptions(provider.GlobalOptions));

            IncrementalValueProvider<ImmutableArray<FileGroup>> group =
                context
                .AdditionalTextsProvider
                .Where(static af => af.Path.EndsWith(".resx"))
                .Collect()
                .SelectMany((files, ct) => files.Group(ct))
                .Collect();

            IncrementalValueProvider<(GlobalOptions options, ImmutableArray<FileGroup> fileGroups)> valueProvider =
                globalOptions
                .Combine(group)
                .Select((entry, _) => (entry.Left, entry.Right));

            context.RegisterSourceOutput(valueProvider, (ctx, value) =>
            {
                foreach (var group in value.fileGroups)
                {
                    ctx.CancellationToken.ThrowIfCancellationRequested();

                    (string Name, string SourceCode) file = group.GenerateFile(value.options, ctx.CancellationToken);
                    ctx.AddSource(file.Name, file.SourceCode);
                }
            });
        }
    }
}
