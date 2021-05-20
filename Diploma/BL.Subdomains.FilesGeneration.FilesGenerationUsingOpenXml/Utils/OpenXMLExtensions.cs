using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlPowerTools;
using System;
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

        internal static List<OpenXmlElement> CloneAllBodyChildElements(this WordprocessingDocument wpd, bool deep)
        {
            return wpd.MainDocumentPart.Document.Body.Select(n => n.CloneNode(deep)).ToList();
        }

        internal static T FindNodeWhichContainsText<T>(this Body body, string nodeInnerText) where T : OpenXmlElement
        {
            return body.ChildElements.FirstOrDefault(e => e.InnerText.Contains(nodeInnerText)) as T;
        }

        /// <summary>
        /// Find a node by inner text oldNodeInnerText, a then replace this node with newNodes
        /// </summary>
        /// <param name="body"></param>
        /// <param name="oldNodeInnerText"></param>
        /// <param name="newNodes"></param>
        /// <returns></returns>
        internal static Body ReplaceNode(this Body docBody, string oldNodeInnerText, List<OpenXmlElement> newNodes)
        {
            var oldNode = docBody.FindNodeWhichContainsText<OpenXmlElement>(oldNodeInnerText) ?? 
                throw new ArgumentException("Can't find an old node for replacing. " +
                $"Value of argument oldNodeInnerText is \"{oldNodeInnerText}\"");

            docBody.ReplaceNode(oldNode, newNodes);

            return docBody;
        }

        internal static Body ReplaceNode(this Body docBody, OpenXmlElement oldNode, List<OpenXmlElement> newNodes)
        {
            var nodeForInserting = oldNode;

            foreach (var node in newNodes)
            {
                nodeForInserting = nodeForInserting.InsertAfterSelf(node.CloneNode(true));
            }

            oldNode.Remove();

            return docBody;
        }

    }
}
