using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;
using System.Collections.Generic;
using System.Linq;

namespace BL.Subdomains.FilesGeneration.FilesGenerationUsingOpenXml.Utils
{
    internal static class OpenXMLExtensions
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

        internal static T FindNodeWhichContainsText<T>(this Body body, string nodeInnerText) where T : OpenXmlElement
        {
            return body.ChildElements.FirstOrDefault(e => e.InnerText.Contains(nodeInnerText)) as T;
        }


    }
}
