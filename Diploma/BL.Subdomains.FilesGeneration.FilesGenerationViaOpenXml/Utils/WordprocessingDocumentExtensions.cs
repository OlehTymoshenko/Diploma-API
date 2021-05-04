using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System.Collections.Generic;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal static class WordprocessingDocumentExtensions
    {
        internal static WordprocessingDocument ReplaceText(this WordprocessingDocument wpd, string oldValue, string newValue, bool matchCase)
        {
            TextReplacer.SearchAndReplace(wpd, oldValue, newValue, matchCase);

            return wpd;
        }

        internal static WordprocessingDocument ReplaceText(this WordprocessingDocument wpd, Dictionary<string, string> pairsForReplace, bool matchCase)
        {
            foreach (var pair in pairsForReplace)
            {
                TextReplacer.SearchAndReplace(wpd, pair.Key, pair.Value, matchCase);
            }

            return wpd;
        }


    }
}
