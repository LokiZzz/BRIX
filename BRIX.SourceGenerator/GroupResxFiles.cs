using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace BRIX.SourceGenerator
{
    public static class GroupResxFiles
    {
        public static List<FileGroup> Group(this IReadOnlyList<AdditionalText> files, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<AdditionalText, List<AdditionalText>>();
            var orderedFiles = files.OrderBy(f => Path.GetFileNameWithoutExtension(f.Path));

            foreach (var file in orderedFiles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var path = file.Path;
                var pathName = Path.GetDirectoryName(path);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                var baseName = GetBaseName(path);

                if (fileNameWithoutExtension == baseName)
                {
                    result[file] = new List<AdditionalText>();
                    continue;
                }

                var key = result.Keys.FirstOrDefault(file =>
                        Path.GetDirectoryName(file.Path) == pathName &&
                        Path.GetFileNameWithoutExtension(file.Path) == baseName);

                if (key is not null)
                {
                    result[key].Add(file);
                }
            }

            return result
                .Select(pair => new FileGroup(pair.Key, pair.Value))
                .ToList();
        }

        // Code from: https://github.com/dotnet/ResXResourceManager/blob/0ec11bae232151400a5a8ca7b9835ac063c516d0/src/ResXManager.Model/ResourceManager.cs#L267
        public static bool IsValidLanguageName(string? languageName)
        {
            try
            {
                if (string.IsNullOrEmpty(languageName))
                    return false;

                // pseudo-locales:
                if (languageName!.StartsWith("qps-", StringComparison.Ordinal))
                    return true;

                var culture = new CultureInfo(languageName);

                while (!culture.IsNeutralCulture)
                {
                    culture = culture.Parent;
                }

                return culture.LCID != 4096;
            }
            catch
            {
                return false;
            }
        }

        // Code from: https://github.com/dotnet/ResXResourceManager/blob/0ec11bae232151400a5a8ca7b9835ac063c516d0/src/ResXManager.Model/ProjectFileExtensions.cs#L77
        public static string GetBaseName(string filePath)
        {
            var name = Path.GetFileNameWithoutExtension(filePath);
            var innerExtension = Path.GetExtension(name);
            var languageName = innerExtension.TrimStart('.');

            return IsValidLanguageName(languageName) ? Path.GetFileNameWithoutExtension(name) : name;
        }
    }
}
